using Microsoft.AspNetCore.Mvc;
using Task2.Data;
using Task2.Models;

namespace Task2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BalancesController : ServiceControllerBase
{
    private readonly IBalancesRepo _balancesRepo;

    public BalancesController(IUsersRepo usersRepo, ITokensRepo tokensRepo, IBalancesRepo balancesRepo) : base(usersRepo, tokensRepo)
    {
        _balancesRepo = balancesRepo;
    }
    
    [Route("create")]
    [HttpPost]
    public ActionResult CreateBalance(string token, int userId, float amount)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        _balancesRepo.CreateBalance(userId, amount);
        _balancesRepo.SaveChanges();
        
        return Ok();
    }
    
    [Route("add")]
    [HttpPost]
    public ActionResult AddBalance(string token, string userLogin, float amount)
    {
        if (!IsAdmin(token)) return Unauthorized();

        var user = _usersRepo.GetUserByLogin(userLogin);
        
        _balancesRepo.AddBalance(user.Id, amount);
        _balancesRepo.SaveChanges();
        
        return Ok();
    }
    
    [Route("subtract")]
    [HttpPost]
    public ActionResult SubtractBalance(string token, int userId, float amount)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        _balancesRepo.SubtractBalance(userId, amount);
        _balancesRepo.SaveChanges();
        
        return Ok();
    }
    
    [Route("get/{userId}")]
    [HttpGet]
    public ActionResult<float> GetBalance(string token, int userId)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        return Ok(_balancesRepo.GetBalance(userId));
    }
    
    [Route("get")]
    [HttpGet]
    public ActionResult<Balance> GetBalance(string token)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        return Ok(_balancesRepo.GetAllBalances());
    }
    
    [Route("my")]
    [HttpGet]
    public ActionResult<float> GetMyBalance(string token)
    {
        int userId = _tokensRepo.GetUserIdByToken(token);
        if (userId == -1) return Unauthorized();
        
        return Ok(_balancesRepo.GetBalance(userId));
    }
}