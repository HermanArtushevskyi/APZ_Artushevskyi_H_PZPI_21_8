using Task2.Models;

namespace Task2.Data;

public class TokensRepo : ITokensRepo
{
    private ServiceDbContext _context;

    public TokensRepo(ServiceDbContext context)
    {
        _context = context;
    }

    public bool SaveChanges() => _context.SaveChanges() > 0;

    public IEnumerable<Token> GetAllTokens()
    {
        return _context.TokensSet.ToList();
    }

    public int GetUserIdByToken(string token)
    {
        var Token = _context.TokensSet.FirstOrDefault(t => t.Value == token);
        if (Token == null) return -1;
        return Token.UserId;
    }

    public void CreateToken(Token token)
    {
        if (token == null) Results.StatusCode(400);
        _context.TokensSet.Add(token);
        _context.SaveChanges();
    }

    public void DeleteToken(string value)
    {
        if (value == null) Results.StatusCode(400);
        var token = _context.TokensSet.FirstOrDefault(t => t.Value == value);
        if (token == null) Results.StatusCode(404);
        _context.TokensSet.Remove(token);
    }
}