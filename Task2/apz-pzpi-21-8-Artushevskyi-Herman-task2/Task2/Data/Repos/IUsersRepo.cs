using Task2.Models;

namespace Task2.Data;

public interface IUsersRepo
{
    public bool SaveChanges();

    public IEnumerable<User> GetAllUsers();
    public User GetUserByLogin(string login);
    public User GetUserById(int id);
    public void CreateUser(User user);
    public bool IsAdmin(string login);
    public bool LoginExists(string login);
    public void DeleteUser(string login);
}