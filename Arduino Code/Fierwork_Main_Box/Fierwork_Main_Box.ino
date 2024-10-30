#include <Wire.h>
#include <Centipede.h>
#include <SoftwareSerial.h>

Centipede CS;

SoftwareSerial HC12(A1, A0);
char HC12ByteIn;
String HC12ReadBuffer = "";
boolean HC12End = false;

// Pin Checking
const int analogPins[] = { }; // 4, 5, 6, 9, 10, 11
const int firePins[] = { 1, 2, 3, 14, 13, 12 };
const int fiveVPins[] = { 8, 7 };
int previousValues[6];
int analognumPins = sizeof(analogPins) / sizeof(analogPins[0]);
int firenumPins = sizeof(firePins) / sizeof(firePins[0]);
int fiveVnumPins = sizeof(fiveVPins) / sizeof(fiveVPins[0]);

int satelliteID = 1;

// Timing
struct Count { int id; int value; };
Count counts[3];
int countSize = 0;

#pragma region Timing
void addNewCount(int id, int value) {
    bool stop = false;
    for (int i = 0; i < countSize; i++) {
        if (id == counts[i].id) {
            counts[i].value = value;
            stop = true;
            Serial.println("Ping");
        }
    }
    if (!stop) {
        Count newCount = { id, value };
        counts[countSize] = newCount;
        countSize++;
    }
}
int CountLength = 5;
void incrementAllCounts() {
    for (int i = 0; i < countSize; i++) {
        counts[i].value++;
        if (counts[i].value >= CountLength) {
            CS.digitalWrite(counts[i].id, LOW);
            removeCount(counts[i].id);
        }
    }
}
void removeCount(int id) {
    for (int i = 0; i < countSize; i++) {
        if (counts[i].id == id) {
            for (int j = i; j < countSize - 1; j++) {
                counts[j] = counts[j + 1];
            }
            countSize--;
            return;
        }
    }
}
#pragma endregion

void setup() {
    Serial.begin(9600);

    HC12.begin(9600);
    HC12ReadBuffer.reserve(64);

    Wire.begin();
    CS.initialize();

    pinMode(A2, OUTPUT);
    digitalWrite(A2, HIGH);

    for (int i = 0; i < analognumPins; i++) {
        CS.pinMode(analogPins[i], INPUT);
        previousValues[i] = -1;
    }
    for (int i = 0; i < firenumPins; i++) {
        CS.pinMode(firePins[i], OUTPUT);
    }
    for (int i = 0; i < fiveVnumPins; i++) {
        CS.pinMode(fiveVPins[i], OUTPUT);
        CS.digitalWrite(fiveVPins[i], HIGH);
    }
}

bool switchlamp = false;

void loop() {
#pragma region HC12
    while (HC12.available()) {
        HC12ByteIn = HC12.read();
        Serial.write(HC12ByteIn);
        HC12ReadBuffer += char(HC12ByteIn);
        if (HC12ByteIn == '\n') {
            HC12End = true;
        }
    }
    while (Serial.available()) {
        HC12.write(Serial.read());
    }
#pragma endregion
#pragma region Inputs
    if (HC12End) {
        HC12ReadBuffer.trim();
        char expectedString[50];
        sprintf(expectedString, "[%d] Get All Settings", satelliteID);
        if (strcmp(HC12ReadBuffer.c_str(), "Stop") == 0 || strcmp(HC12ReadBuffer.c_str(), "0") == 0) {
            for (int i = 0; i < firenumPins; i++) {
                CS.digitalWrite(firePins[i], LOW);
            }
            for (int i = 0; i < countSize; i++) {
                removeCount(counts[i].id);
            }
        }
        else if (strcmp(HC12ReadBuffer.c_str(), "Satellite Check In") == 0) {
            char cstr[20];
            sprintf(cstr, "[ID: %d]", satelliteID);
            HC12.write(cstr);
        }
        else if (strcmp(HC12ReadBuffer.c_str(), expectedString) == 0) {
        }
        else {
            int pinNumber = atoi(HC12ReadBuffer.c_str());
            if (pinNumber >= 1 + ((satelliteID - 1) * 120) && pinNumber <= 120 + ((satelliteID - 1) * 120)) {
                int pinIndex = pinNumber - 1 - ((satelliteID - 1) * 120);
                if (pinIndex < firenumPins) {
                    CS.digitalWrite(firePins[pinIndex], HIGH);
                    addNewCount(firePins[pinIndex], 0);
                }
            }
        }
        HC12ReadBuffer = "";
        HC12End = false;
    }
#pragma endregion
#pragma region Connection Update
    for (int i = 0; i < analognumPins; i++) {
        int currentValue = round(analogRead(analogPins[i]) * (5.0 / 1023.0));
        switch (currentValue) {
        case 0:
            currentValue = 0;
            break;
        case 1:
        case 2:
        case 3:
            currentValue = 3;
            break;
        case 4:
        case 5:
            currentValue = 5;
            break;
        default:
            break;
        }
        if (previousValues[i] != currentValue)
        {
            char cstr[20];
            switch (currentValue) {
            case 0:
                sprintf(cstr, "[%d.%d: On]", satelliteID, i);
                break;
            case 1:
            case 2:
            case 3:
                sprintf(cstr, "[%d.%d: Off]", satelliteID, i);
                break;
            case 4:
            case 5:
                sprintf(cstr, "[%d.%d: Disconnected]", satelliteID, i);
                break;
            default:
                break;
            }
            previousValues[i] = currentValue;
            Serial.println(cstr);
            strcat(cstr, "\r\n");
            HC12.write(cstr);
        }
    }
#pragma endregion
    delay(100);
    incrementAllCounts();
}