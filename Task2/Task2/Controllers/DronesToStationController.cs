using Microsoft.AspNetCore.Mvc;
using Task2.Data;
using Task2.Models;

namespace Task2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DronesToStationController : ServiceControllerBase
{
    private readonly IDroneStationRepo _droneStationRepo;
    
    public DronesToStationController(IUsersRepo usersRepo, ITokensRepo tokensRepo, IDroneStationRepo droneStationRepo) : base(usersRepo, tokensRepo)
    {
        _droneStationRepo = droneStationRepo;
    }
    
    [Route("all")]
    [HttpGet]
    public ActionResult<IEnumerable<DroneToStation>> GetAllDronesToStations(string token)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        return Ok(_droneStationRepo.GetAllDroneToStations());
    }
    
    [Route("delete")]
    [HttpDelete]
    public ActionResult DeleteDroneToStation(string token, int id)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        _droneStationRepo.Delete(id);
        _droneStationRepo.SaveChanges();
        
        return Ok();
    }
}