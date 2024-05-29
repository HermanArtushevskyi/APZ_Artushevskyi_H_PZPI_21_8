using Microsoft.AspNetCore.Mvc;
using Task2.Data;
using Task2.DTO;
using Task2.Models;

namespace Task2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DronesController : ServiceControllerBase
{
    private readonly IDronesRepo _dronesRepo;
    private readonly IDroneStationRepo _droneStationRepo;
    private readonly IStationsRepo _stationsRepo;
    private readonly IBalancesRepo _balancesRepo;
    private readonly IUsersRepo _usersRepo;
    private readonly ITokensRepo _tokensRepo;
    private readonly IDroneModelsRepo _droneModelsRepo;
    
    private const float MAX_DISTANCE = 500f;
    
    public DronesController(IUsersRepo usersRepo, ITokensRepo tokensRepo, IDronesRepo dronesRepo, IDroneStationRepo droneStationRepo, IBalancesRepo balancesRepo, IStationsRepo stationsRepo, IDroneModelsRepo droneModelsRepo) : base(usersRepo, tokensRepo)
    {
        _usersRepo = usersRepo;
        _tokensRepo = tokensRepo;
        _dronesRepo = dronesRepo;
        _droneStationRepo = droneStationRepo;
        _balancesRepo = balancesRepo;
        _stationsRepo = stationsRepo;
        _droneModelsRepo = droneModelsRepo;
    }
    
    [Route("all")]
    [HttpGet]
    public ActionResult<IEnumerable<Drone>> GetAllDrones(string token)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        return Ok(_dronesRepo.GetAllDrones());
    }
    
    [Route("create")]
    [HttpPost]
    public ActionResult<DroneDto> CreateDrone(string token, DroneDto droneDto, int stationId)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        if (!_stationsRepo.IsStationExists(stationId)) return BadRequest();
        
        Drone drone = new Drone()
        {
            SerialNumber = droneDto.SerialNumber,
            ModelId = droneDto.ModelId,
            StatusId = (int) DroneStatus.Status.Idle,
            CurrentUserId = -1
        };
        
        
        _dronesRepo.CreateDrone(drone);
        _dronesRepo.SaveChanges();

        int droneId = _dronesRepo.GetDroneBySerialNumber(drone.SerialNumber).Id;
        
        DroneToStation droneToStation = new DroneToStation()
        {
            DroneId = droneId,
            StationId = stationId
        };
        
        _droneStationRepo.CreateDroneStation(droneToStation);
        _droneStationRepo.SaveChanges();
        
        return Ok(drone);
    }
    
    [Route("delete")]
    [HttpDelete]
    public ActionResult DeleteDrone(string token, int id)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        _dronesRepo.DeleteDrone(id);
        _dronesRepo.SaveChanges();
        
        return Ok();
    }
    
    [Route("station")]
    [HttpGet]
    public ActionResult<Station> GetDroneStation(string token, int droneId)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        if (!_dronesRepo.IsDroneExists(droneId)) return NotFound();
        
        return Ok(_droneStationRepo.GetDroneStation(droneId));
    }
    
    [Route("model/create")]
    [HttpPost]
    public ActionResult<DroneModel> CreateDroneModel(string token, DroneModelDto droneModelDto)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        DroneModel droneModel = new DroneModel()
        {
            Name = droneModelDto.Name,
            Price = droneModelDto.Price
        };
        
        _droneModelsRepo.CreateDroneModel(droneModel);
        _droneModelsRepo.SaveChanges();
        
        return Ok(droneModel);
    }
    
    [Route("model/delete")]
    [HttpDelete]
    public ActionResult DeleteDroneModel(string token, int id)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        _droneModelsRepo.DeleteDroneModel(id);
        _droneModelsRepo.SaveChanges();
        
        return Ok();
    }
    
    [Route("model/all")]
    [HttpGet]
    public ActionResult<IEnumerable<DroneModel>> GetAllDroneModels(string token)
    {
        if (!IsAdmin(token)) return Unauthorized();
        
        return Ok(_droneModelsRepo.GetAllDroneModels());
    }
    
    [Route("rent")]
    [HttpPost]
    public ActionResult<string> RentDrone(string token, int modelId, double longitude, double latitude)
    {
        if (_tokensRepo.GetUserIdByToken(token) == -1) return Unauthorized();
        DroneModel droneModel = _droneModelsRepo.GetDroneModelById(modelId);
        float balance = _balancesRepo.GetBalance(_tokensRepo.GetUserIdByToken(token));
        if (balance < droneModel.Price) return BadRequest();

        IEnumerable<Station> stations = _stationsRepo.GetAllStations();
        
        stations = stations.Where(s => GetDistance(s.Longitude, s.Latitude, longitude, latitude) <= MAX_DISTANCE);
        
        if (!stations.Any()) return NoContent();
        
        IEnumerable<DroneToStation> droneToStations = _droneStationRepo.GetAllDroneToStations();
        
        droneToStations = droneToStations.Where(ds => stations.Any(s => s.Id == ds.StationId));

        Drone? drone = FindSuitableDrone(droneToStations, modelId);
        
        if (drone == null) return NoContent();
        
        drone.CurrentUserId = _tokensRepo.GetUserIdByToken(token);
        drone.StatusId = (int) DroneStatus.Status.Rented;

        _dronesRepo.UpdateDrone(drone);
        
        _balancesRepo.SubtractBalance(_tokensRepo.GetUserIdByToken(token), droneModel.Price);
        _dronesRepo.SaveChanges();
        _balancesRepo.SaveChanges();
        
        return Ok(drone.SerialNumber);
    }

    private Drone? FindSuitableDrone(IEnumerable<DroneToStation> droneToStations, int modelId)
    {
        foreach (DroneToStation droneToStation in droneToStations)
        {
            Drone? drone = _dronesRepo.GetDroneById(droneToStation.DroneId);
            if (drone == null) return null;
            if (drone.ModelId == modelId && drone.StatusId == (int) DroneStatus.Status.Idle)
            {
                return drone;
            }
        }

        return null;
    }

    [Route("return")]
    [HttpPost]
    public ActionResult ReturnDrone(string token, string serialNumber)
    {
        int userId = _tokensRepo.GetUserIdByToken(token);
        Drone drone = _dronesRepo.GetDroneBySerialNumber(serialNumber);
        if (drone.CurrentUserId != userId) return BadRequest();

        drone.CurrentUserId = -1;
        drone.StatusId = (int) DroneStatus.Status.Idle;

        _dronesRepo.UpdateDrone(drone);
        _dronesRepo.SaveChanges();
        
        return Ok();
    }

    /// <summary>
    /// Get a distance between 2 points in form of longitude and latitude
    /// </summary>
    /// <returns>Distance</returns>
    private float GetDistance(double longitude1, double latitude1, double longitude2, double latitude2)
    {
        return (float)Math.Sqrt(Math.Pow(longitude1 - longitude2, 2) + Math.Pow(latitude1 - latitude2, 2));
    }
}