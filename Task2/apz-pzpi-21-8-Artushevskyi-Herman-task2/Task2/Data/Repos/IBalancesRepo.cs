using Task2.Models;

namespace Task2.Data;

public interface IBalancesRepo
{
    void SaveChanges();
    void CreateBalance(int userId, float amount);
    void AddBalance(int userId, float amount);
    void SubtractBalance(int userId, float amount);
    float GetBalance(int userId);
    IEnumerable<Balance> GetAllBalances();
}