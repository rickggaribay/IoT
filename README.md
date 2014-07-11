Demos from my Internet of Things talk at VSLive Chicago on May 8th, 2014.

These demos are based on the definition of the Command message pattern in the "Service Assisted Communication" for Connected Devices paper that Clemens Vasters (@clemensv) at Microsoft published in February: http://bit.ly/saccv

1. Default Communication Model with Arduino - demonstrates the default communication model whereby the Arduino provides its own API (via a Web Server adapted by zoomcat). Commands are sent from the command source to the device in a point to point manner. Demo: https://www.youtube.com/watch?v=LG2d3g15QLY&list=UUi3K_ytcpblN7U6-8ljGE6A

2. Brokered Device Communication with Netduino Plus 2 - demonstrates an evolution from the point to point default communication model to a brokered approach to issuing device commands using MQTT. This demo uses the excellent M2MQTT library by WEB MVP Paolo Patierno (@ppatierno) as well as the MQTT plug-in for RabbitMQ (both on-premise and RabbitMQ hosted). Demo: https://www.youtube.com/watch?v=H1K6AAO2ed8&list=UUi3K_ytcpblN7U6-8ljGE6A

3. Service-Assisted Device-Direct Commands over Azure Service Bus - applies the fundamental service assisted communications concepts evolving the brokered example to leverage Azure Service Bus using the Device Direct pattern (as opposed to Custom Gateway). As with the brokered model, the device communicates with a single endpoint in an outbound manner, but does not require a dedicated socket connection as with MQTT implicitly addressing occasionally disconnected scenarios, message durability, etc. Demo: https://www.youtube.com/watch?v=XTvv6sAzVRM&list=UUi3K_ytcpblN7U6-8ljGE6A

4. Service-Assisted Device-Direct Commands on the Azure Device Gateway/Reykjavik - demonstrates the culmination of work dating back to June 2012 (in which Clemens Vasters first shared the concept of Service-Assisted Communications) which is now available as a reference architecture and fully functional code base for customers ready to adopt an IoT strategy today. I will post this demo as soon as the framework becomes public. Stay tuned.  
