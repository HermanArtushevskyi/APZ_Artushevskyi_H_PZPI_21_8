namespace Task2.Services;

public interface IPasswordHasher
{
    public (string, string) HashPassword(string password);
    public bool VerifyPassword(string password, string passwordHash, string passwordSalt);
}