using Task2.Models;

namespace Task2.Data;

public interface IStationsRepo
{
    public bool SaveChanges();
    
    public IEnumerable<Station> GetAllStations();
    public Station GetStationById(int id);
    public bool IsStationExists(int id);
    public void CreateStation(Station station);
    public void UpdateStation(Station station);
    public void DeleteStation(int id);
}