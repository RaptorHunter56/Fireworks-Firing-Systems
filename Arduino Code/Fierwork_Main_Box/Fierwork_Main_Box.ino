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

const int maxFirePins = 31;
const int maxFiveVPins = 1;

int firePins[maxFirePins] = { /*4*/29,15,6, 4,16,21, 17,26,9, 11,30,5, 22,3,0, 27,20,23, 10,25,1, 24,14,2, 28,8,13, 18,12,19};
int fiveVPins[maxFiveVPins] = { 7 };
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
int CountLength = 50;
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
#pragma region HC12
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
void HC12Send(const String& message) {
    char dataToSend[100]; // Buffer to hold the data to send

    // Ensure the message fits in the buffer
    if (message.length() < sizeof(dataToSend)) {
        message.toCharArray(dataToSend, sizeof(dataToSend)); // Convert String to char array
        HC12.println(dataToSend); // Send data to HC-12
    } else {
        Serial.println("Message too long to send.");
    }
}
#pragma endregion

const int SIZE = (maxFirePins + (2)) / 3;
int ppin = 99;
String SEND[SIZE] = { };
void addElement(String arr[], String newValue) {
    for (int i = 0; i < SIZE; i++) {
        if (arr[i] == "") { // Check for the first empty spot
            arr[i] = newValue; // Add the new value
            return; // Exit the function after adding
        }
    }
}
bool isArrayEmpty(String arr[]) {
    for (int i = 0; i < SIZE; i++) {
        if (arr[i] != "") { // Check if any element is not empty
            return false; // Found a non-empty element
        }
    }
    return true; // All elements are empty
}
void trimNonPrintable(char* str) {
    char trimmed[100]; // Temporary array to hold trimmed characters
    int index = 0;

    // Iterate through the original string
    for (int i = 0; str[i] != '\0'; i++) {
        // Check if the character is printable
        if (str[i] >= 32 && str[i] <= 126) {
            trimmed[index++] = str[i]; // Add printable character to trimmed
        }
    }

    trimmed[index] = '\0'; // Null-terminate the trimmed string
    strcpy(str, trimmed);   // Copy trimmed string back to original
}

const int MESSAGE_SEND_INTERVAL = 10;
int messageDelayCounter = 0;

void setup() {
    Serial.begin(9600);

    HC12.begin(9600);
    HC12ReadBuffer.reserve(64);

    Wire.begin();
    CS.initialize();

    pinMode(A2, OUTPUT);
    digitalWrite(A2, HIGH);

    savePins();
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
        if (HC12ByteIn >= 32 && HC12ByteIn <= 126 || HC12ByteIn == '\n') {
            Serial.write(HC12ByteIn);
        }
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
        sprintf(expectedString, "[ID:%d] Get All Settings", satelliteID);
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
            sprintf(cstr, "[ID:%d]", satelliteID);
            strcat(cstr, "\n");
            HC12.write(cstr);
        }
        else if (strcmp(trimToAllowedCharacters(HC12ReadBuffer).c_str(), expectedString) == 0) {
            char finalStr[200];
            sprintf(finalStr, "[ID:%d|mF:%d]", satelliteID, maxFirePins);
            addElement(SEND, finalStr);

            const int chunkSize = 3; // Number of pins to send in each message
            for (int i = 0; i < maxFirePins; i += chunkSize) {
                char firePinsChunk[100];
                firePinsChunk[0] = '\0';
                for (int j = 0; j < chunkSize && (i + j) < maxFirePins; j++) {
                    char temp[20];
                    sprintf(temp, "%d", firePins[i + j]);
                    if (j == 0)
                        sprintf(firePinsChunk + strlen(firePinsChunk), "%s", temp);
                    else
                        sprintf(firePinsChunk + strlen(firePinsChunk), ",%s", temp);
                }
                trimNonPrintable(firePinsChunk);
                // Now send firePinsChunk
                char finalStr[200];
                sprintf(finalStr, "[ID:%d|f:%s]", satelliteID, firePinsChunk);
                addElement(SEND, finalStr);
            }

            ppin = 0;
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
            strcat(cstr, "\n");
            HC12.write(cstr);
        }
    }
#pragma endregion
    if (messageDelayCounter >= MESSAGE_SEND_INTERVAL && ppin <= SIZE - 1) {
        char msgsend[100];
        //shift(SEND).toCharArray(msgsend, sizeof(msgsend));
        SEND[ppin].toCharArray(msgsend, sizeof(msgsend));
        Serial.println(msgsend);
        strcat(msgsend, "\n");
        HC12Send(msgsend);
        messageDelayCounter = 0; // Reset the counter after sending
        ppin++;
    }
    messageDelayCounter++;

    delay(100);
    incrementAllCounts();
}