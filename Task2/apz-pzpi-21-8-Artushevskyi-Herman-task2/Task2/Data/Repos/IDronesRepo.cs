using Task2.Models;

namespace Task2.Data;

public interface IDronesRepo
{
    public void SaveChanges();
    public void CreateDrone(Drone drone);
    public void UpdateDrone(Drone drone);
    public bool IsDroneExists(int droneId);
    public void DeleteDrone(int droneId);
    public Drone GetDroneById(int droneId);
    public IEnumerable<Drone> GetAllDrones();
    public Drone? GetDroneBySerialNumber(string serialNumber);
}