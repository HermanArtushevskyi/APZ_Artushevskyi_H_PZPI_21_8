using Microsoft.AspNetCore.Mvc;
using Task2.Data;
using Task2.DTO;
using Task2.Models;
using Task2.Services;

namespace Task2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ServiceControllerBase
{
    private readonly IUsersRepo _usersRepo;
    private readonly ITokensRepo _tokensRepo;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenCreator _tokenCreator;
    private readonly IBalancesRepo _balancesRepo;

    public UsersController(IUsersRepo usersRepo, ITokensRepo tokensRepo,
        IPasswordHasher hasher, ITokenCreator tokenCreator, IBalancesRepo balancesRepo) : base(usersRepo, tokensRepo)
    {
        _usersRepo = usersRepo;
        _tokensRepo = tokensRepo;
        _hasher = hasher;
        _tokenCreator = tokenCreator;
        _balancesRepo = balancesRepo;
    }

    [Route("register")]
    [HttpPost]
    public ActionResult<UserReadDto> Register(UserCreateDto dto)
    {
        Console.WriteLine($"=> {nameof(UsersController)}::{nameof(Register)}: Registering user {dto.Login}...");

        (string, string) passwordData = _hasher.HashPassword(dto.Password);

        User user = new User()
        {
            Login = dto.Login,
            Email = dto.Email,
            PasswordHash = passwordData.Item1,
            PasswordSalt = passwordData.Item2,
            IsAdmin = false
        };
        
        try
        {
            if (_usersRepo.LoginExists(user.Login)) return BadRequest();
            _usersRepo.CreateUser(user);
            _usersRepo.SaveChanges();

            int userId = _usersRepo.GetUserByLogin(user.Login).Id;
            _balancesRepo.CreateBalance(userId, 0);
            _balancesRepo.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }

        return Ok(new UserReadDto(user));
    }
    
    
    [Route("login")]
    [HttpPost()]
    public ActionResult<Token> Login(string login, string password)
    {
        Console.WriteLine($"=> {nameof(UsersController)}::{nameof(Login)}: Logging in user {login} {password}...");
        var user = _usersRepo.GetUserByLogin(login);
        if (user == null) return NotFound();
        if (!_hasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt)) return Unauthorized();
        Token token = _tokenCreator.CreateToken(user.Id);
        _tokensRepo.CreateToken(token);
        
        return Ok(token);
    }
    
    [Route("logout")]
    [HttpPost()]
    public ActionResult Logout(string value)
    {
        Console.WriteLine($"=> {nameof(UsersController)}::{nameof(Logout)}: Logging out user with token {value}...");
        var userId = _tokensRepo.GetUserIdByToken(value);
        if (userId == -1) return NotFound();
        _tokensRepo.DeleteToken(value);
        _tokensRepo.SaveChanges();
        return Ok();
    }
    
    [Route("tokens")]
    [HttpGet()]
    public ActionResult<IEnumerable<Token>> GetAllTokens(string token)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        Console.WriteLine($"=> {nameof(UsersController)}::{nameof(GetAllTokens)}: Getting all tokens...");
        var tokens = _tokensRepo.GetAllTokens();
        if (tokens == null) return NotFound();
        return Ok(tokens);
    }
    
    [Route("tokens")]
    [HttpPost]
    public ActionResult<Token> CreateToken(string token, int userId, string value)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        var newToken = new Token()
        {
            Value = value,
            UserId = userId
        };
        
        _tokensRepo.CreateToken(newToken);
        _tokensRepo.SaveChanges();
        return Ok(newToken);
    }

    [Route("user")]
    [HttpGet]
    public ActionResult<IEnumerable<UserReadDto>> GetAllUsers(string token)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        var users = _usersRepo.GetAllUsers();
        if (users == null) return NotFound();
        List<UserReadDto> dtos = new();
        foreach (var user in users)
        {
            dtos.Add(new UserReadDto(user));
        }
        return Ok(dtos);
    }
    
    [Route("user/{token}")]
    [HttpGet]
    public ActionResult<UserReadDto> GetUserByToken(string token)
    {
        var userId = _tokensRepo.GetUserIdByToken(token);
        if (userId == -1) return NotFound();
        var user = _usersRepo.GetUserById(userId);
        if (user == null) return NotFound();
        return Ok(new UserReadDto(user));
    }
    
    [Route("user")]
    [HttpDelete]
    public ActionResult DeleteUserByLogin(string token, string value)
    {
        if (!IsAdmin(token)) return Unauthorized();

        var user = _usersRepo.GetUserByLogin(value);
        if (user == null) return NotFound();
        _usersRepo.DeleteUser(user.Login);
        _usersRepo.SaveChanges();
        return Ok();
    }
    
    [Route("balance/{userId}")]
    [HttpGet]
    public ActionResult<Balance> GetBalance(string token, int userId)
    {
        bool canAccess = false;
        
        if (IsAdmin(token)) canAccess = true;
        if (userId == _tokensRepo.GetUserIdByToken(token)) canAccess = true;
        
        if (!canAccess) return Unauthorized();
        
        var balance = _balancesRepo.GetBalance(userId);
        if (balance == null) return NotFound();
        return Ok(balance);
    }
}