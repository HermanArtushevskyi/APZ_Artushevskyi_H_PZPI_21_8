using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Task2.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 256;
    private const int HashSize = 512;
    private const int Iterations = 1000;
    
    public (string, string) HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize / 8);
        
        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, HashSize / 8));
        return (hash, Convert.ToBase64String(salt));
    }

    public bool VerifyPassword(string password, string passwordHash, string passwordSalt)
    {
        byte[] salt = Convert.FromBase64String(passwordSalt);
        string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, Iterations, HashSize / 8));
        return hash == passwordHash;
    }
}