using Microsoft.AspNetCore.Mvc;
using Task2.Data;
using Task2.DTO;
using Task2.Models;

namespace Task2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StationsController : ServiceControllerBase
{
    private readonly IStationsRepo _stationsRepo;
    
    public StationsController(IStationsRepo stationsRepo, IUsersRepo usersRepo, ITokensRepo tokensRepo) : base(usersRepo, tokensRepo)
    {
        _stationsRepo = stationsRepo;
    }
    
    [Route("all")]
    [HttpGet]
    public ActionResult<IEnumerable<Station>> GetAllStations(string token)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        return Ok(_stationsRepo.GetAllStations());
    }
    
    [Route("create")]
    [HttpPost]
    public ActionResult<StationDto> CreateStation(string token, StationDto stationDto)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        Station station = new Station()
        {
            Name = stationDto.Name,
            Latitude = stationDto.Latitude,
            Longitude = stationDto.Longitude,
            Capacity = stationDto.Capacity
        };
        
        _stationsRepo.CreateStation(station);
        _stationsRepo.SaveChanges();
        
        return Ok(station);
    }
    
    [Route("delete")]
    [HttpDelete]
    public ActionResult DeleteStation(string token, int id)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        _stationsRepo.DeleteStation(id);
        _stationsRepo.SaveChanges();
        
        return Ok();
    }
}