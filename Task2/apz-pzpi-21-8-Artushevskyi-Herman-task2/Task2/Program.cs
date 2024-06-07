using Task2.BackgroundServices;
using Task2.Data;
using Task2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IUsersRepo, UsersRepo>();
builder.Services.AddScoped<ITokensRepo, TokensRepo>();
builder.Services.AddScoped<IStationsRepo, StationsRepo>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenCreator, TokenCreator>();
builder.Services.AddScoped<IBalancesRepo, BalancesRepo>();
builder.Services.AddScoped<IDroneStationRepo, DroneStationRepo>();
builder.Services.AddScoped<IDronesRepo, DronesRepo>();
builder.Services.AddScoped<IDroneModelsRepo, DroneModelsRepo>();
builder.Services.AddScoped<IAccelerationRepo, AccelerationRepo>();
builder.Services.AddScoped<MQTTData>();
builder.Services.AddControllers();
builder.Services.AddHostedService<DroneCheckService>();

builder.SetupDatabase();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();