using Task2.Models;

namespace Task2.Data;

public class UsersRepo : IUsersRepo
{
    private ServiceDbContext _context;

    public UsersRepo(ServiceDbContext context)
    {
        _context = context;
    }

    public bool SaveChanges() => _context.SaveChanges() > 0;

    public IEnumerable<User> GetAllUsers()
    {
        return _context.UsersSet.ToList();
    }

    public User? GetUserByLogin(string login)
    {
        return _context.UsersSet.FirstOrDefault(user => user.Login == login);
    }

    public User GetUserById(int id)
    {
        return _context.UsersSet.FirstOrDefault(user => user.Id == id);
    }

    public void CreateUser(User user)
    {
        if (user == null) Results.StatusCode(400);
        _context.UsersSet.Add(user);
    }

    public bool IsAdmin(string login)
    {
        var user = _context.UsersSet.FirstOrDefault(u => u.Login == login);
        if (user == null) return false;
        return user.IsAdmin;
    }

    public bool LoginExists(string login)
    {
        return _context.UsersSet.Any(u => u.Login == login);
    }

    public void DeleteUser(string login)
    {
        if (login == null) Results.StatusCode(400);
        var user = _context.UsersSet.FirstOrDefault(u => u.Login == login);
        if (user == null) Results.StatusCode(404);
        _context.UsersSet.Remove(user);
    }
}