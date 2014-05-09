Demos from my Internet of Things talk at VSLive Chicago on May 8th, 2014.

These demos are based on the definition of the Command message type and based on the "Service Assisted Communications" work that Clemens Vasters (@clemensv) at Microsoft has done in the M2M and device to cloud space. For more details, please refer to this blog post: http://bit.ly/saccv

1. Default Communication Model with Arduino - demonstrates the default communication model whereby the Arduino provides its own API (via a Web Server adapted by zoomcat). Commands are sent from the command source to the device in a point to point manner. 

2. Brokered Device Communication with Netduino Plus 2 - demonstrates an evolution from the point to point default communication model to a brokered approach to issuing device commands using MQTT. This demo uses the excellent M2MQTT library by WEB MVP Paolo Patierno (@ppatierno) as well as the MQTT plug-in for RabbitMQ (both on-premise and RabbitMQ hosted). 

3. Service-Assisted Device-Direct Commands over Azure Service Bus - applies the fundamental service assisted communications concepts evolving the brokered example to leverage Azure Service Bus using the Device Direct pattern (as opposed to Custom Gateway). As with the brokered model, the device communicates with a single endpoint in an outbound manner, but does not require a dedicated socket connection as with MQTT implicitly addressing occasionally disconnected scenarios, message durability, etc. 

Please note that the 4th demo (Reykjavik/Device Gateway) is not available as the product is currently only available via direct engagements and not available publically. If your organization would be interested in learning more about Reykjavik, please visit http://neudesic.com/iot and we will follow up.  
