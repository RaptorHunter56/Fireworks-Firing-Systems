#include <SoftwareSerial.h>

// HC12
SoftwareSerial HC12(A0, A1);    // HC-12 TX Pin, HC-12 RX Pin
char HC12ByteIn;                // Temporary variable
String HC12ReadBuffer = "";     // Read/Write Buffer 1 for HC12
boolean HC12End = false;        // Flag to indicate End of HC12 String

// Read Conection
const int analogPins[] = { A2, A3, A4, A5, A6, A7 };        // Array of analog pins to read from
char cstr[10];                                              // Character array to store the converted value
int previousValues[6];                                      // Array to store previous values for each pin
int numPins = sizeof(analogPins) / sizeof(analogPins[0]);   // Number of pins


void setup() {
    //Start Serial port
    Serial.begin(9600);           // Serial port to computer
    HC12.begin(9600);             // Serial port to HC12
    HC12ReadBuffer.reserve(64);   // Reserve 64 bytes for Serial message input

    // Start Read Conection values
    for (int i = 0; i < numPins; i++) {
        previousValues[i] = -1; // Initialize previous values to -1
    }
}

void loop() {
    // HC12 Write
    while (HC12.available()) {              // If HC-12 has data
        HC12ByteIn = HC12.read();           // Store each character from rx buffer in byteIn
        Serial.write(HC12ByteIn);           // Send the data to Serial monitor
        HC12ReadBuffer += char(HC12ByteIn); // Write each character of byteIn to HC12ReadBuffer
        if (HC12ByteIn == '\n') {           // At the end of the line
            HC12End = true;                 // Set HC12End flag to true
        }
    }
    // HC12 Read
    while (Serial.available()) {    // If Serial monitor has data
        HC12.write(Serial.read());  // Send that data to HC-12
    }
    /// Read for
    if (HC12End) {
        if (strcmp(HC12ReadBuffer.c_str(), "Check All") == 0) { // Execute printAllValues() if buffer contains "Check All"
            printAllValues();
        }
        HC12ReadBuffer = "";
        HC12End = false;
    }
    ///

    // Read Conection - Send Conection
    for (int i = 0; i < numPins; i++) {
        int currentValue = analogRead(analogPins[i]) * (5.0 / 1023.0);  // Read and convert analog value
        itoa(currentValue, cstr, 10);                                   // Convert value to string

        if (currentValue != previousValues[i]) {                    // Check if value has changed
            sprintf(cstr, "Pin A%d: %s\n", i + 2, currentValue);    // Print pin label, pin number (A2 = 2, A3 = 3, etc.)
            Serial.println(cstr);    
            HC12.write(cstr);
            previousValues[i] = currentValue;   // Update previous value
        }
    }

    delay(100); // Wait for 100ms before taking the next reading
}

// Read All Conection - Send All Conections
void printAllValues() {
    char cstr[100];     // Character array to store the converted value
    char tempStr[20];   // Temporary string to store individual pin values
    cstr[0] = '\0';     // Initialize cstr to empty string

    for (int i = 0; i < numPins; i++) {
        int currentValue = analogRead(analogPins[i]) * (5.0 / 1023.0);  // Read and convert analog value
        sprintf(tempStr, "[Pin A%d: %d],", i + 2, currentValue);        // Print pin label, pin number (A2 = 2, A3 = 3, etc.)
        strcat(cstr, tempStr);                                          // Append tempStr to cstr
    }

    Serial.println(cstr);       // Print the entire string
    HC12.write(cstr + "\r\n");  // Send the entire string
}