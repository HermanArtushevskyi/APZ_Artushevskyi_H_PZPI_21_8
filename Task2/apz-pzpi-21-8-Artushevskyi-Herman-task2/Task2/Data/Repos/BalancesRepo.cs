using Task2.Models;

namespace Task2.Data;

public class BalancesRepo : IBalancesRepo
{
    private readonly ServiceDbContext _context;

    public BalancesRepo(ServiceDbContext context)
    {
        _context = context;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public void CreateBalance(int userId, float amount)
    {
        Balance balance = new Balance()
        {
            UserId = userId,
            Amount = amount
        };

        _context.BalancesSet.Add(balance);
    }

    public void AddBalance(int userId, float amount)
    {
        Balance balance = _context.BalancesSet.FirstOrDefault(b => b.UserId == userId);
        balance.Amount += amount;
    }

    public void SubtractBalance(int userId, float amount)
    {
        Balance balance = _context.BalancesSet.FirstOrDefault(b => b.UserId == userId);
        balance.Amount -= amount;
    }

    public float GetBalance(int userId)
    {
        Balance balance = _context.BalancesSet.FirstOrDefault(b => b.UserId == userId);
        return balance.Amount;
    }

    public IEnumerable<Balance> GetAllBalances()
    {
        return _context.BalancesSet.ToList();
    }
}