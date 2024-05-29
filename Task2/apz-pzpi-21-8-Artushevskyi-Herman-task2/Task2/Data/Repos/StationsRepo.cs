using Task2.Models;

namespace Task2.Data;

public class StationsRepo : IStationsRepo
{
    private ServiceDbContext _context;

    public StationsRepo(ServiceDbContext context)
    {
        _context = context;
    }

    public bool SaveChanges() => _context.SaveChanges() > 0;

    public IEnumerable<Station> GetAllStations()
    {
        return _context.StationsSet.ToList();
    }

    public Station GetStationById(int id)
    {
        return _context.StationsSet.FirstOrDefault(station => station.Id == id);
    }

    public bool IsStationExists(int id)
    {
        return _context.StationsSet.Any(station => station.Id == id);
    }

    public void CreateStation(Station station)
    {
        if (station == null) Results.StatusCode(400);
        _context.StationsSet.Add(station);
    }

    public void UpdateStation(Station station)
    {
        if (station == null) Results.StatusCode(400);
        _context.StationsSet.Update(station);
    }

    public void DeleteStation(int id)
    {
        if (id == null) Results.StatusCode(400);
        var station = _context.StationsSet.FirstOrDefault(s => s.Id == id);
        if (station == null) Results.StatusCode(404);
        _context.StationsSet.Remove(station);
    }
}