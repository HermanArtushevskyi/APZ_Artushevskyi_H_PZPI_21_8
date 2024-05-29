using Task2.Models;

namespace Task2.Services;

public interface ITokenCreator
{
    public Token CreateToken(int userId);
}