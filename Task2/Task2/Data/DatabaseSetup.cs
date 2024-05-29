using Microsoft.EntityFrameworkCore;
using Task2.Models;

namespace Task2.Data;

public static class DatabaseSetup
{
    public static void SetupDatabase(this WebApplicationBuilder builder)
    {
        bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        
        Console.WriteLine($"=> {nameof(DatabaseSetup)}::{nameof(SetupDatabase)} Is development: {isDevelopment}");
        
        IServiceCollection services = builder.Services.AddDbContext<ServiceDbContext>(options =>
        {
            //if (!isDevelopment)
            //{
                string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            //}
            //else if (isDevelopment) options.UseInMemoryDatabase("UsersDB");
        });

        //if (isDevelopment) PrepareDatabase(services.BuildServiceProvider().GetRequiredService<ServiceDbContext>());
        //else
            ApplyMigrations(services.BuildServiceProvider().GetRequiredService<ServiceDbContext>());
    }

    private static void PrepareDatabase(ServiceDbContext context)
    {
        if (!context.UsersSet.Any())
        {
            Console.WriteLine($"=> {nameof(DatabaseSetup)}::{nameof(PrepareDatabase)}: Seeding database...");
            context.UsersSet.Add(new User()
            {
                Login = "LonelyAlone", Email = "germanartushevskii@gmail.com", PasswordHash = "password",
                PasswordSalt = "salt", IsAdmin = true
            });
            
            context.UsersSet.Add(new User()
            {
                Login = "John", Email = "johndoe@gmail.com", PasswordHash = "johnp",
                PasswordSalt = "johnsalt", IsAdmin= false
                
            });

            context.SaveChanges();
        }
        else
        {
            Console.WriteLine($"=> {nameof(DatabaseSetup)}::{nameof(PrepareDatabase)}: Database already seeded.");
        }
    }

    public static void ApplyMigrations(ServiceDbContext context)
    {
        try
        {
            context.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("=> {nameof(DatabaseSetup)}::{nameof(ApplyMigrations)}: Error while applying migrations.");
            Console.WriteLine(e);
        }
    }
}