#include <Wire.h>
#include <Centipede.h>

Centipede CS;

void setup() {
  Wire.begin(); // start I2C
 
  CS.initialize(); // set all registers to default
 
  CS.portMode(0, 0b0000000000000000); // set all pins on chip 0 to output (0 to 15)
  CS.portMode(1, 0b0000000000000000); // set all pins on chip 1 to output (0 to 15)
  CS.portMode(2, 0b0000000000000000); // set all pins on chip 2 to output (0 to 15)
  CS.portMode(3, 0b0000000000000000); // set all pins on chip 3 to output (0 to 15)
}

void loop() {
  // put your main code here, to run repeatedly:

}
