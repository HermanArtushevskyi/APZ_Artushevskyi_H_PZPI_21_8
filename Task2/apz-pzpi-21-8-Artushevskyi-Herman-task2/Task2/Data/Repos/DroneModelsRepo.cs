using Task2.Models;

namespace Task2.Data;

public class DroneModelsRepo : IDroneModelsRepo
{
    private ServiceDbContext _context;

    public DroneModelsRepo(ServiceDbContext context)
    {
        _context = context;
    }

    public IEnumerable<DroneModel> GetAllDroneModels()
    {
        return _context.DroneModelsSet.ToList();
    }

    public DroneModel GetDroneModelById(int id)
    {
        return _context.DroneModelsSet.FirstOrDefault(droneModel => droneModel.Id == id);
    }

    public DroneModel GetDroneModelByName(string name)
    {
        return _context.DroneModelsSet.FirstOrDefault(droneModel => droneModel.Name == name);
    }

    public void CreateDroneModel(DroneModel droneModel)
    {
        if (droneModel == null) Results.StatusCode(400);
        _context.DroneModelsSet.Add(droneModel);
    }

    public void DeleteDroneModel(int id)
    {
        if (id == 0) Results.StatusCode(400);
        var droneModel = _context.DroneModelsSet.FirstOrDefault(droneModel => droneModel.Id == id);
        if (droneModel == null) Results.StatusCode(404);
        _context.DroneModelsSet.Remove(droneModel);
    }

    public bool SaveChanges() => _context.SaveChanges() > 0;
}