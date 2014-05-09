//zoomkat 4-05-12
//web LED code
//for use with IDE 1.0
//open serial monitor to see what the arduino receives
//use the \ slash to escape the " in the html (or use ') 
//address will look like http://192.168.1.102:84 when submited
//for use with W5100 based ethernet shields
//turns pin 5 on/off

#include <SPI.h>
#include <Ethernet.h>

byte mac[] = { 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED }; //physical mac address
byte ip[] = { 206, 205, 190, 155}; // arduino server ip in lan
byte gateway[] = { 206, 205, 188, 1 }; // internet access via router gateway
byte subnet[] = { 255, 255, 252, 0 }; //subnet mask
EthernetServer server(8088); //arduino server port

String readString; 

//////////////////////

void setup(){

  pinMode(9, OUTPUT); //pin selected to control
  digitalWrite(9, LOW);    // set pin 9 low

  //start Ethernet
  Ethernet.begin(mac, ip, gateway, subnet);
  server.begin();


  //enable serial data print 
  Serial.begin(9600); 
  Serial.println("servertest1"); // so I can keep track of what is loaded
}

void loop(){
  // Create a client connection
  EthernetClient client = server.available();
  if (client) {
    while (client.connected()) {
      if (client.available()) {
        char c = client.read();

        //read char by char HTTP request
        if (readString.length() < 100) {

          //store characters to string 
          readString += c; 
          Serial.print(c); //print what server receives to serial monitor
        } 

        //if HTTP request has ended
        if (c == '\n') {

          ///////////////
          Serial.println(readString);

          //now output HTML data header

          client.println("HTTP/1.1 200 OK");
          client.println("Content-Type: text/html");
          client.println();

          client.println("<HTML>");
          client.println("<HEAD>");
          client.println("<TITLE>Arduino Command Server</TITLE>");
          client.println("</HEAD>");
          client.println("<BODY>");
          
          client.println("<H1>Arduino Web Server Up and Running</H1>");
          client.println("<BR>");

          client.println("<H2>Ready to begin processing commands...</H2>");

          client.println("<BR>");

          delay(1);
      
          client.println("<ul>");
          
          if(readString.indexOf("LED=on") >0)//checks for on
          {
            client.println("<li><H3>ON Command Received</H3></li>");
            digitalWrite(9, HIGH);    // set pin 5 high
            Serial.println("Led On");
          }
          if(readString.indexOf("LED=off") >0)//checks for off
          {
            client.println("<li><H3>OFF Command Received</H3></li>");
            digitalWrite(9, LOW);    // set pin 5 low
            Serial.println("Led Off");
          }
          
          client.println("</ul>");

          client.println("</BODY>");
          client.println("</HTML>");
          
          //stopping client
          client.stop();
          
          //clear for next read
          readString="";

        }
      }
    }
  }
} 
