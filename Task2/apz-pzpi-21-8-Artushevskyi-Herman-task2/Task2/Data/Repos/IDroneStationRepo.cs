using Task2.Models;

namespace Task2.Data;

public interface IDroneStationRepo
{
    public void SaveChanges();
    public void CreateDroneStation(DroneToStation droneStation);
    public void ChangeDroneStation(int droneID, int newStationId);
    public Station GetDroneStation(int droneId);
    public IEnumerable<DroneToStation> GetAllDroneToStations();

    public void Delete(int id);
}