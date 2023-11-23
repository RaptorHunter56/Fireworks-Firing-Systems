//#include <Wire.h>
//#include <Centipede.h>
#include <SoftwareSerial.h>

//Centipede CS; // create Centipede object

SoftwareSerial HC12(A0, A1); // HC-12 TX Pin, HC-12 RX Pin
char HC12ByteIn;                                // Temporary variable
String HC12ReadBuffer = "";                     // Read/Write Buffer 1 for HC12
boolean HC12End = false;                        // Flag to indiacte End of HC12 String

void setup() {
  //Wire.begin(); // start I2C
 
  //CS.initialize(); // set all registers to default
 
  //CS.portMode(0, 0b0000000000000000); // set all pins on chip 0 to output (0 to 15)
  //CS.portMode(1, 0b0000000000000000); // set all pins on chip 1 to output (16 to 31)
  //CS.portMode(2, 0b0000000000000000); // set all pins on chip 2 to output (32 to 47)
  //CS.portMode(3, 0b0000000000000000); // set all pins on chip 3 to output (48 to 36)

  Serial.begin(9600);             // Serial port to computer
  HC12.begin(9600);               // Serial port to HC12

  pinMode(2, OUTPUT); 
  pinMode(3, OUTPUT); 
  pinMode(4, OUTPUT); 
  pinMode(5, OUTPUT); 

  HC12ReadBuffer.reserve(64);                   // Reserve 64 bytes for Serial message input

  
  digitalWrite(2, LOW);
  digitalWrite(3, LOW);
  digitalWrite(4, LOW);
  digitalWrite(5, LOW);
}

void loop() {
  while (HC12.available()) {        // If HC-12 has data
    HC12ByteIn = HC12.read();                   // Store each character from rx buffer in byteIn
    Serial.write(HC12ByteIn);      // Send the data to Serial monitor
    HC12ReadBuffer += char(HC12ByteIn);         // Write each character of byteIn to HC12ReadBuffer
    if (HC12ByteIn == '\n') {                   // At the end of the line
      HC12End = true;                           // Set HC12End flag to true
    }
  }
  while (Serial.available()) {      // If Serial monitor has data
    HC12.write(Serial.read());      // Send that data to HC-12
  }

  switch (HC12ReadBuffer.toInt()) {
    case 1:
      digitalWrite(2, LOW);
      digitalWrite(3, LOW);
      digitalWrite(4, LOW);
      digitalWrite(5, LOW);
      break;
    case 2:
      digitalWrite(2, HIGH);
      digitalWrite(3, LOW);
      digitalWrite(4, LOW);
      digitalWrite(5, LOW);
      break;
    case 3:
      digitalWrite(2, LOW);
      digitalWrite(3, HIGH);
      digitalWrite(4, LOW);
      digitalWrite(5, LOW);
      break;
    case 4:
      digitalWrite(2, LOW);
      digitalWrite(3, LOW);
      digitalWrite(4, HIGH);
      digitalWrite(5, LOW);
      break;
    case 5:
      digitalWrite(2, LOW);
      digitalWrite(3, LOW);
      digitalWrite(4, LOW);
      digitalWrite(5, HIGH);
      break;
    default:
      break;
  }
  HC12ReadBuffer = "";

  // put your main code here, to run repeatedly:
  //for (int i = 0; i < 16; i++) {
    //CS.digitalWrite(i, HIGH);
    //Serial.print("HIGH\r\n");
  //}
  //delay(1000);
 
  //for (int i = 0; i < 16; i++) {
    //CS.digitalWrite(i, LOW);
    //Serial.print("LOW\r\n");
  //} 
  //delay(1000);
}
