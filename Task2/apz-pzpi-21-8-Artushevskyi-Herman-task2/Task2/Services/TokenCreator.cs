using Task2.Data;
using Task2.Models;

namespace Task2.Services;

public class TokenCreator : ITokenCreator
{
    private readonly ITokensRepo _tokensRepo;

    public TokenCreator(ITokensRepo tokensRepo)
    {
        _tokensRepo = tokensRepo;
    }
    
    public Token CreateToken(int userId)
    {
        Token token = new Token()
        {
            UserId = userId
        };

        string value = GetTokenValue();

        while (_tokensRepo.GetUserIdByToken(value) != -1) value = GetTokenValue();

        token.Value = value;
        
        return token;
    }

    private string GetTokenValue() => Guid.NewGuid().ToString();
}