using DefaultNamespace;
using Task2.Models;

namespace Task2.Data;

public interface IAccelerationRepo
{
    IEnumerable<Acceleration> GetAllAccelerationData();
    Acceleration GetAccelerationDataById(int id);
    Acceleration? GetAccelerationDataBySerialNumber(string serialNumber);
    void CreateAccelerationData(Acceleration accelerationData);
    void DeleteAccelerationData(string serialNumber);
    bool SaveChanges();
}

public class AccelerationRepo : IAccelerationRepo
{
    private ServiceDbContext _context;

    public AccelerationRepo(ServiceDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Acceleration> GetAllAccelerationData()
    {
        return _context.AccelerationsSet.ToList();
    }

    public Acceleration GetAccelerationDataById(int id)
    {
        return _context.AccelerationsSet.FirstOrDefault(accelerationData => accelerationData.Id == id);
    }

    public Acceleration? GetAccelerationDataBySerialNumber(string serialNumber)
    {
        bool exist = _context.AccelerationsSet.Any(accelerationData => accelerationData.SerialNumber == serialNumber);

        if (!exist) return null;
        else
            return _context.AccelerationsSet.OrderBy(acc => acc.Id).Last(accelerationData => accelerationData.SerialNumber == serialNumber);
    }

    public void CreateAccelerationData(Acceleration accelerationData)
    {
        if (accelerationData == null) Results.StatusCode(400);
        _context.AccelerationsSet.Add(accelerationData);
    }

    public void DeleteAccelerationData(string serialNumber)
    {
        if (serialNumber == null) Results.StatusCode(400);
        var accelerationData = _context.AccelerationsSet.Where(accelerationData => accelerationData.SerialNumber == serialNumber);
        foreach (var acceleration in accelerationData)
        {
            _context.AccelerationsSet.Remove(acceleration);
        }
    }

    public bool SaveChanges() => _context.SaveChanges() > 0;
}
