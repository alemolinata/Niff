#include <RFduinoGZLL.h>
device_t role = DEVICE0;

int r0 = 0;      //value of select pin at the 4051 (s0)
int r1 = 0;      //value of select pin at the 4051 (s1)
int r2 = 0;      //value of select pin at the 4051 (s2)
long loopCount = 0;

const int pinS0 = 5;  // digital pins for reading multiplexer (s0)
const int pinS1 = 4;  // digital pins for reading multiplexer (s1)
const int pinS2 = 3;  // digital pins for reading multiplexer (s2)

const int pinMP = 6;  // pin for multiplexer, reads legs positions
const int pinCPS = 2; // core power stone pin

int binaryReadings = 0;
int prevBinaryReadings = 0;

int corePowerStone;
int connectors[8];
int buttonPressed = 0;

void setup() {
  pinMode(pinS0, OUTPUT);    // s0
  pinMode(pinS1, OUTPUT);    // s1
  pinMode(pinS2, OUTPUT);    // s2

  pinMode(pinMP, INPUT);
  pinMode(pinCPS, INPUT);

  //Serial.begin(9600);
  RFduinoGZLL.begin(role);
}

void loop() {
  readConnectors();
  loopCount ++;
}

void readConnectors(){
  //String readings = "";
  for (int i=0; i< 8; i++){
    // select the bit  
    r0 = bitRead(i,0);    
    r1 = bitRead(i,1);      
    r2 = bitRead(i,2);

    digitalWrite(pinS0, r0);
    digitalWrite(pinS1, r1);
    digitalWrite(pinS2, r2);

    connectors[i] = analogRead(pinMP);
    //readings = readings + String(connectors[i]) + ", ";
    if(connectors[i] > 600)
      connectors[i] = 1;
    else
      connectors[i] = 0;
  }
  corePowerStone = analogRead(pinCPS);

  if(corePowerStone > 120 && corePowerStone < 190){
    corePowerStone = 1;
    buttonPressed = 0;
  }
  else if(corePowerStone > 240 && corePowerStone < 300){
    corePowerStone = 1;
    buttonPressed = 1;
  }
  else if(corePowerStone > 480 && corePowerStone < 540){
    corePowerStone = 2;
    buttonPressed = 0;
  }
  else if(corePowerStone > 640 && corePowerStone < 710){
    corePowerStone = 2;
    buttonPressed = 1;
  }
  else{
    corePowerStone = 0;
    buttonPressed = 0;
  }
  
  //readings = readings + String(corePowerStone);
  //Serial.println(readings);
  binaryReadings = convertToBinary();
  if(binaryReadings != prevBinaryReadings || loopCount > 100){
    //Serial.println(String(binaryReadings, BIN));
    RFduinoGZLL.sendToHost(binaryReadings);
    loopCount = 0;
  }
  prevBinaryReadings = binaryReadings;
}

int convertToBinary(){
  int result = 16;
  if(connectors[0] == 1 || connectors[3] == 1){
    if(connectors[0] == connectors[3]){
      result += B1000;
    }
    else if(connectors[1] == 1 || connectors[4] == 1){
      result += B1100;
    }
    else if(connectors[2] == 1 || connectors[5] == 1){
      result += B1010;
    }
    else{
      result += B1001;
    }
  }
  else if(connectors[1] == 1 || connectors[4] == 1){
    if(connectors[1] == connectors[4]){
      result += B100;
    }
    else if(connectors[2] == 1 || connectors[5] == 1){
      result += B110;
    }
    else{
      result += B101;
    }
  }
  else if(connectors[2] == 1 || connectors[5] == 1){
    if(connectors[2] == connectors[5]){
      result += B10;
    }
    else{
      result += B11;
    }
  }
  result = result << 2;
  result += corePowerStone;
  result = result << 1;
  result += buttonPressed;
  return result;
}
