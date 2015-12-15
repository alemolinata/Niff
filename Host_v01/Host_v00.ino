#include <RFduinoGZLL.h>

device_t role = HOST;
int binaryReadings = 0;

int connectors[6];
int corePowerStone;

void setup() {
  RFduinoGZLL.begin(role);
  Serial.begin(9600);
}

void loop() {

}

void RFduinoGZLL_onReceive(device_t device, int rssi, char *data, int len)
{
    if (device == DEVICE0){
      char binaryReadingsChar = data[0];
      binaryReadings = (int)binaryReadingsChar;
      if(binaryReadings > 127 && binaryReadings < 256)
        Serial.write(binaryReadings);
        //Serial.println(String(binaryReadings, BIN));
    } 
}

void readFromBinary(int binaryCode){
    int i = 0;
    int bit = B10000000;
    for( i = 0 ; i < 6 ; i ++ ){
        connectors[i] = (binaryCode & bit) / bit;
        bit = bit >> 1; //<-
    }
    corePowerStone = binaryCode & B11;
}
