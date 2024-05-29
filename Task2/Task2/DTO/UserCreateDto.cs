namespace Task2.DTO;

public class UserCreateDto
{
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public UserCreateDto(string login, string email, string password)
    {
        Login = login;
        Email = email;
        Password = password;
    }
}