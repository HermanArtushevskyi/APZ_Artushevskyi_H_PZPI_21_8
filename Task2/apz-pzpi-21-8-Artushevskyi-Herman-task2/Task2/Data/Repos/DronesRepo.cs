using Task2.Models;

namespace Task2.Data;

public class DronesRepo : IDronesRepo
{
    public ServiceDbContext _context;
    
    public DronesRepo(ServiceDbContext context)
    {
        _context = context;
    }
    
    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void CreateDrone(Drone drone)
    {
        _context.DronesSet.Add(drone);
    }

    public void UpdateDrone(Drone drone)
    {
        _context.DronesSet.Update(drone);
    }

    public bool IsDroneExists(int droneId)
    {
        return _context.DronesSet.Any(d => d.Id == droneId);
    }

    public void DeleteDrone(int droneId)
    {
        var drone = _context.DronesSet.FirstOrDefault(d => d.Id == droneId);
        _context.DronesSet.Remove(drone);
    }

    public Drone GetDroneById(int droneId)
    {
        return _context.DronesSet.FirstOrDefault(d => d.Id == droneId);
    }

    public IEnumerable<Drone> GetAllDrones()
    {
        return _context.DronesSet.ToList();
    }

    public Drone? GetDroneBySerialNumber(string serialNumber)
    {
        bool exist = _context.DronesSet.Any(d => d.SerialNumber == serialNumber);
        
        if (!exist) return null;
        else
            return _context.DronesSet.FirstOrDefault(d => d.SerialNumber == serialNumber);
    }
}