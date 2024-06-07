using Task2.Models;

namespace Task2.Data;

public interface IDroneModelsRepo
{
    IEnumerable<DroneModel> GetAllDroneModels();
    DroneModel GetDroneModelById(int id);
    DroneModel GetDroneModelByName(string name);
    void CreateDroneModel(DroneModel droneModel);
    void DeleteDroneModel(int id);
    bool SaveChanges();
}