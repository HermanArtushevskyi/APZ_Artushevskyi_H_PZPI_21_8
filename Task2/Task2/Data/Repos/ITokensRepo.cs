using Task2.Models;

namespace Task2.Data;

public interface ITokensRepo
{
    public bool SaveChanges();

    public IEnumerable<Token> GetAllTokens();
    public int GetUserIdByToken(string token);
    public void CreateToken(Token token);
    public void DeleteToken(string value);
}