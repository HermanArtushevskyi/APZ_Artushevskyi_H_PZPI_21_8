#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <ArduinoMqttClient.h>
#include <WiFi.h>
#include <Wire.h>
#include <WiFiClient.h>

#define ssid "Wokwi-GUEST"
#define password ""

const int btnPin = 3;
int btnStatus = LOW;

float lastXAcc = 0.0;
float lastYAcc = 0.0;
float lastZAcc = 0.0;

const char* mqttBroker = "broker.hivemq.com";
int mqttPort = 1883;
const char* mqttUsername = "Admin";
const char* mqttPassword = "Admin111";
const char* returnTopic = "drone/return";
const char* accelerationTopic = "drone/accelerometer";
const char* availableTopic = "drone/available";

const char* serialId = "11";

WiFiClient wfClient;
MqttClient mqttClient(wfClient);

Adafruit_MPU6050 mpu;

void setup_wifi() {
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    // failed, retry
    delay(500);
  }
}

void setup_mqtt(){
  if (!mqttClient.connect(mqttBroker, mqttPort)) {
    Serial.print("[MQTT]: MQTT connection failed! Error code = ");
    Serial.println(mqttClient.connectError());
  }

}

void setup_mpu(){
  while (!mpu.begin()) {
    delay(1000);
  }
}

void setup_btn(){
  pinMode(btnPin, INPUT);
}

void setup() {
  Serial.begin(115200);

  setup_btn();
  Serial.println("Buttons are ready!");
  setup_mpu();
  Serial.println("Mpu is ready!");
  setup_wifi();
  Serial.println("Wifi is ready!");
  setup_mqtt();
  Serial.println("Drone is ready!");
}

sensors_event_t a, g, temp;

void loop() {
  mqttClient.poll();
  btnStatus = digitalRead(btnPin);
  if (temp.temperature > 40.0)
  {
    mqttClient.beginMessage(returnTopic);
    mqttClient.print("{\"serialNumber\": ");
    mqttClient.print(serialId);
    mqttClient.print("}");
    mqttClient.endMessage();
  }

  mpu.getEvent(&a, &g, &temp);
  if (a.acceleration.x != lastXAcc ||
      a.acceleration.y != lastYAcc ||
      a.acceleration.z != lastZAcc)
      {
        lastXAcc = a.acceleration.x;
        lastYAcc = a.acceleration.y;
        lastZAcc = a.acceleration.z;

        // Building json message
        // {
        // "x": 0.0,
        // "y": 0.0,
        // "z": 0.0,
        // "serialNumber": null
        // }
        mqttClient.beginMessage(accelerationTopic);
        mqttClient.print("{");
        mqttClient.print("\"x\": ");
        mqttClient.print(lastXAcc);
        mqttClient.print(",");
        mqttClient.print("\"y\": ");
        mqttClient.print(lastYAcc);
        mqttClient.print(",");
        mqttClient.print("\"z\": ");
        mqttClient.print(lastZAcc);
        mqttClient.print(",");
        mqttClient.print("\"serialNumber\": ");
        mqttClient.print(serialId);
        mqttClient.print("}");
        mqttClient.endMessage();
      }

  delay(600);
}
