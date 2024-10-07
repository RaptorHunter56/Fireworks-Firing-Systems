#include <SoftwareSerial.h>

SoftwareSerial HC12(A0, A1);
char HC12ByteIn;
String HC12ReadBuffer = "";
boolean HC12End = false;

const int analogPins[] = { A2, A3, A4 };
int previousValues[6];
int numPins = sizeof(analogPins) / sizeof(analogPins[0]);

void setup() {
	Serial.begin(9600);
	HC12.begin(9600);
	HC12ReadBuffer.reserve(64);

    for (int i = 0; i < numPins; i++) {
        pinMode(analogPins[i], INPUT);
        previousValues[i] = -1;
    }
}

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

    for (int i = 0; i < numPins; i++) {
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
                sprintf(cstr, "[1.%d: On]", i);
                break;
            case 1:
            case 2:
            case 3:
                sprintf(cstr, "[1.%d: Off]", i);
                break;
            case 4:
            case 5:
                sprintf(cstr, "[1.%d: Disconnected]", i);
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

    delay(500);
}