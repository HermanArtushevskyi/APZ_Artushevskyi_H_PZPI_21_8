using System.Text;
using DefaultNamespace;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using Task2.Data;
using Task2.Models;
using Task2.Services;

namespace Task2.BackgroundServices;

public class DroneCheckService : BackgroundService
{
    private readonly IAccelerationRepo _accelerationRepo;
    private readonly IDronesRepo _dronesRepo;
    private readonly MQTTData _mqttData;

    public DroneCheckService(IAccelerationRepo accelerationRepo, MQTTData mqttData, IDronesRepo dronesRepo)
    {
        _accelerationRepo = accelerationRepo;
        _mqttData = mqttData;
        _dronesRepo = dronesRepo;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CheckMQTTMessages(stoppingToken);
        
        await Task.CompletedTask;
    }

    private async Task CheckMQTTMessages(CancellationToken cancellationToken)
    {
        var factory = new MqttFactory();
        var mqttClient = factory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(_mqttData.ClientId)
            .WithTcpServer(_mqttData.MqttBroker, _mqttData.MqttPort)
            .WithCleanSession()
            .Build();
        
        mqttClient.ConnectAsync(options, cancellationToken);
        
        mqttClient.ConnectedAsync += (async e =>
        {
            await mqttClient.SubscribeAsync(_mqttData.AvailabilityTopic);
            await mqttClient.SubscribeAsync(_mqttData.ReturnTopic);
            await mqttClient.SubscribeAsync(_mqttData.AccelerometerTopic);
        });
        
        mqttClient.ApplicationMessageReceivedAsync += delegate(MqttApplicationMessageReceivedEventArgs args)
        {
            var topic = args.ApplicationMessage.Topic;
            var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);
            Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
            if (topic == _mqttData.ReturnTopic)
            {
                ReturnHandle(payload);
            }
            else if (topic == _mqttData.AccelerometerTopic)
            {
                AccelerometerHandle(payload);
            }
            else if (topic == _mqttData.AvailabilityTopic)
            {
                AvailableHandle(payload);
            }
            return Task.CompletedTask;
        };
    }

    private void AccelerometerHandle(string payload)
    {
        AccelerationData data = JsonConvert.DeserializeObject<AccelerationData>(payload);
        Console.WriteLine($"Acceleration data: X: {data.X}, Y: {data.Y}, Z: {data.Z} Serial number: {data.SerialNumber}");
        _accelerationRepo.DeleteAccelerationData(data.SerialNumber);
        
        _accelerationRepo.CreateAccelerationData(new Acceleration()
        {
            X = data.X,
            Y = data.Y,
            Z = data.Z,
            SerialNumber = data.SerialNumber
        });
        _accelerationRepo.SaveChanges();
    }

    private void ReturnHandle(string payload)
    {
        ReturnDroneData data = JsonConvert.DeserializeObject<ReturnDroneData>(payload);
        
        Console.WriteLine("Drone returned: " + data.SerialNumber);
        if (_dronesRepo.GetDroneBySerialNumber(data.SerialNumber) != null)
        {
            Drone drone = _dronesRepo.GetDroneBySerialNumber(data.SerialNumber);
            drone.CurrentUserId = -1;
            drone.StatusId = (int) DroneStatus.Status.Rest;
            _dronesRepo.UpdateDrone(drone);
            _dronesRepo.SaveChanges();
        }
    }

    private void AvailableHandle(string payload)
    {
        AvailableData data = JsonConvert.DeserializeObject<AvailableData>(payload);
        
        Console.WriteLine("Drone is available: " + data.SerialNumber);
        
        if (_dronesRepo.GetDroneBySerialNumber(data.SerialNumber) != null)
        {
            Drone drone = _dronesRepo.GetDroneBySerialNumber(data.SerialNumber);
            drone.StatusId = (int) DroneStatus.Status.Idle;
            _dronesRepo.UpdateDrone(drone);
            _dronesRepo.SaveChanges();
        }
    }
}