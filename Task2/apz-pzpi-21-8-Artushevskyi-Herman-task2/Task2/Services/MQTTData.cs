namespace Task2.Services;

public class MQTTData
{
    public readonly string MqttBroker = "broker.hivemq.com";
    public readonly int MqttPort = 1883;
    public readonly string ClientId = Guid.NewGuid().ToString();
    public readonly string ReturnTopic = "drone/return";
    public readonly string AccelerometerTopic = "drone/accelerometer";
    public readonly string AvailabilityTopic = "drone/available";
}