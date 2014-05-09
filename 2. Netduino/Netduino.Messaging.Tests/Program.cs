using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT.Rabbit
{
    class Program
    {
        private static int mcount;
        static void Main(string[] args)
        {

            Console.WriteLine("***********PUBLISHER***********");


            //MqttClient client = new MqttClient("dev.rabbitmq.com", 1883, false, null);
            MqttClient client = new MqttClient("localhost", 1883, false, null);
            var state = client.Connect("Client993", "guest", "guest", false, 0, false, null, null, true, 60);

            string strValue = Convert.ToString("On");

            while (true)
            {
                // publish a message with command to turn LED on
                client.Publish("ABC12345/led", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
                mcount++;

                Console.WriteLine(String.Format("Message {0} published.", mcount.ToString()));

                Console.WriteLine("Press enter to send Off command...");
                Console.ReadLine();

                // publish a message with command to turn LED on
                client.Publish("ABC12345/led", Encoding.UTF8.GetBytes("Off"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
                mcount++;

                Console.WriteLine(String.Format("Message {0} published.", mcount.ToString()));

                Console.WriteLine("Press enter to send On comamnd...");
                Console.ReadLine();
            }
        }




    }
}
