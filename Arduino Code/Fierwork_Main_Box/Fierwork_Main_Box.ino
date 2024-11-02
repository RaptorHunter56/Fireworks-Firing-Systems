#include <Wire.h>
#include <Centipede.h>
#include <SoftwareSerial.h>
#include <EEPROM.h>

Centipede CS;

SoftwareSerial HC12(A1, A0);
char HC12ByteIn;
String HC12ReadBuffer = "";
boolean HC12End = false;

// Pin Checking
const int analogPins[] = { }; // 4, 5, 6, 9, 10, 11
int analognumPins = sizeof(analogPins) / sizeof(analogPins[0]);

const int maxFirePins = 6;
const int maxFiveVPins = 2;

int firePins[maxFirePins] = { /*1*/1, 2, 3, /*2*/14, 13, 12 };
int fiveVPins[maxFiveVPins] = { /*1*/8, /*2*/7 };
int previousValues[maxFirePins];

int firenumPins = sizeof(firePins) / sizeof(firePins[0]);;
int fiveVnumPins = sizeof(fiveVPins) / sizeof(fiveVPins[0]);;

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
#pragma region EEPROM
void loadPins() {
    firenumPins = EEPROM.read(0);
    for (int i = 0; i < firenumPins; i++) {
        firePins[i] = EEPROM.read(i + 1);
    }
    fiveVnumPins = EEPROM.read(maxFirePins + 1);
    for (int i = 0; i < fiveVnumPins; i++) {
        fiveVPins[i] = EEPROM.read(maxFirePins + 2 + i);
    }
}
void savePins() {
    EEPROM.write(0, firenumPins);
    for (int i = 0; i < firenumPins; i++) {
        EEPROM.write(i + 1, firePins[i]);
    }
    EEPROM.write(maxFirePins + 1, fiveVnumPins);
    for (int i = 0; i < fiveVnumPins; i++) {
        EEPROM.write(maxFirePins + 2 + i, fiveVPins[i]);
    }
}
#pragma endregion
#pragma region Clean HC12
String trimToAllowedCharacters(String input) {
    if (input.length() == 0) { return input; }
    int startIndex = 0;
    while (startIndex < input.length() &&
        !(input[startIndex] >= '0' && input[startIndex] <= '9') &&  // Check for numbers
        !(input[startIndex] >= 'a' && input[startIndex] <= 'z') &&  // Check for lowercase letters
        !(input[startIndex] >= 'A' && input[startIndex] <= 'Z') &&  // Check for lowercase letters
        input[startIndex] != '[') {                                 // Check for '['
        startIndex++;
    }
    return input.substring(startIndex);
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

    //savePins();
    loadPins();

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

//Serial.println();
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
        sprintf(expectedString, "[ID: %d] Get All Settings", satelliteID);
        if (strcmp(trimToAllowedCharacters(HC12ReadBuffer).c_str(), "Stop") == 0 || strcmp(trimToAllowedCharacters(HC12ReadBuffer).c_str(), "0") == 0) {
            for (int i = 0; i < firenumPins; i++) {
                CS.digitalWrite(firePins[i], LOW);
            }
            for (int i = 0; i < countSize; i++) {
                removeCount(counts[i].id);
            }
        }
        else if (strcmp(trimToAllowedCharacters(HC12ReadBuffer).c_str(), "Satellite Check In") == 0) {
            Serial.println("Check In");
            char cstr[20];
            sprintf(cstr, "[ID: %d]", satelliteID);
            strcat(cstr, "\r\n");
            HC12.write(cstr);
        }
        else if (strcmp(trimToAllowedCharacters(HC12ReadBuffer).c_str(), expectedString) == 0) {
            //firePins
            char firePinsStr[100];
            firePinsStr[0] = '\0';
            for (int i = 0; i < maxFirePins; i++) {
                char temp[20];
                sprintf(temp, "%d", firePins[i]);
                if (i == 0) {
                    sprintf(firePinsStr, "[%s", temp);
                }
                else {
                    sprintf(firePinsStr + strlen(firePinsStr), ", %s", temp);
                }
            }
            sprintf(firePinsStr + strlen(firePinsStr), "]");
            char finalStr[200];
            sprintf(finalStr, "[ID: %d | maxFirePins : %d, firePins: %s]", satelliteID, maxFirePins, firePinsStr);
            strcat(finalStr, "\r\n");
            HC12.write(finalStr);
            Serial.println(finalStr);

            //fiveVPins
            char fiveVPinsStr[100];
            fiveVPinsStr[0] = '\0';
            for (int i = 0; i < maxFiveVPins; i++) {
                char temp[20];
                sprintf(temp, "%d", fiveVPins[i]);
                if (i == 0) {
                    sprintf(fiveVPinsStr, "[%s", temp);
                }
                else {
                    sprintf(fiveVPinsStr + strlen(fiveVPinsStr), ", %s", temp);
                }
            }
            sprintf(fiveVPinsStr + strlen(fiveVPinsStr), "]");
            char finalStrV[200];
            sprintf(finalStrV, "[ID: %d | maxFiveVPins : %d, fiveVPins: %s]", satelliteID, maxFiveVPins, fiveVPinsStr);
            strcat(finalStrV, "\r\n");
            HC12.write(finalStrV);

        }
        else {
            int pinNumber = atoi(trimToAllowedCharacters(HC12ReadBuffer).c_str());
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