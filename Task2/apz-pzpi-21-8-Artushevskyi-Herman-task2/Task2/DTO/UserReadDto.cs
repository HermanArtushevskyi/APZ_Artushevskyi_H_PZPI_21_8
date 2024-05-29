using Task2.Models;

namespace Task2.DTO;

public class UserReadDto
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public bool IsAdmin { get; set; }
    
    public UserReadDto(User user)
    {
        Login = user.Login;
        Email = user.Email;
        PasswordHash = user.PasswordHash;
        PasswordSalt = user.PasswordSalt;
        IsAdmin = user.IsAdmin;
    }
}