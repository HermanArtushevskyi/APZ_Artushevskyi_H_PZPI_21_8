using Task2.Models;

namespace Task2.Data;

public class DroneStationRepo : IDroneStationRepo
{
    private readonly ServiceDbContext _context;
    
    public DroneStationRepo(ServiceDbContext context)
    {
        _context = context;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void CreateDroneStation(DroneToStation droneStation)
    {
        _context.DronesToStationsSet.Add(droneStation);
    }

    public void ChangeDroneStation(int droneID, int newStationId)
    {
        DroneToStation droneStation = _context.DronesToStationsSet.FirstOrDefault(ds => ds.DroneId == droneID);
        droneStation.StationId = newStationId;
    }

    public Station GetDroneStation(int droneId)
    {
        DroneToStation droneStation = _context.DronesToStationsSet.FirstOrDefault(ds => ds.DroneId == droneId);
        return _context.StationsSet.FirstOrDefault(s => s.Id == droneStation.StationId);
    }

    public IEnumerable<DroneToStation> GetAllDroneToStations()
    {
        return _context.DronesToStationsSet;
    }

    public void Delete(int id)
    {
        DroneToStation droneStation = _context.DronesToStationsSet.FirstOrDefault(ds => ds.Id == id);
        _context.DronesToStationsSet.Remove(droneStation);
    }
}