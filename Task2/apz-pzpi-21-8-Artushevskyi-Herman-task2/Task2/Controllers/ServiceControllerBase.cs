using Microsoft.AspNetCore.Mvc;
using Task2.Data;

namespace Task2.Controllers;

public class ServiceControllerBase : ControllerBase
{
    protected readonly IUsersRepo _usersRepo;
    protected readonly ITokensRepo _tokensRepo;
    
    public ServiceControllerBase(IUsersRepo usersRepo, ITokensRepo tokensRepo)
    {
        _usersRepo = usersRepo;
        _tokensRepo = tokensRepo;
    }
    
    protected bool IsAdmin(string token)
    {
        int userId = _tokensRepo.GetUserIdByToken(token);
        if (userId == -1) return false;
        return _usersRepo.GetUserById(userId).IsAdmin;
    }
}