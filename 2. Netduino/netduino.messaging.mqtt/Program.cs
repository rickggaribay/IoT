using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Netduino.Messaging.MQTT
{
    public class Program
    {
        public static OutputPort led; 

        public static void Main()
        {
            led = new OutputPort(Pins.ONBOARD_LED, false);

            Microsoft.SPOT.Hardware.Utility.SetLocalTime(new DateTime(2014, 4, 13));

            Thread.Sleep(10000);

            var networkInterface = NetworkInterface.GetAllNetworkInterfaces()[0];

            if (networkInterface.IPAddress == IPAddress.Any.ToString())
            {

                networkInterface.EnableDhcp();
                networkInterface.EnableDynamicDns();
                networkInterface.RenewDhcpLease();
            }

           
            // create client instance 
            //MqttClient client = new MqttClient("dev.rabbitmq.com", 1883, false, null);
            MqttClient client = new MqttClient("206.205.188.34", 1883, false, null);


            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
            client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;
            client.MqttMsgPublished += client_MqttMsgPublished;



            // subscribe to the topic
            client.Subscribe(new string[] { "ABC12345/led" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

            //Console.ReadLine();
            var state = client.Connect("ABC12345", "guest", "guest", false, 0, false, null, null, true, 60);
        }


        private static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Debug.Print(e.MessageId.ToString());
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = UTF8Encoding.UTF8.GetChars(e.Message);

            string command = new string(message);
            
            Debug.Print("Command is: " + command);

            if (command == "On")
            {
                led.Write(true);

            }
            else
            {
                led.Write(false);

            }

        }

        static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Debug.Print(e.MessageId.ToString());
        }

        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Debug.Print(e.MessageId.ToString());
        }




    }



}
