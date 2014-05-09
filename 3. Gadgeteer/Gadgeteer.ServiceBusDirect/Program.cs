using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Microsoft.SPOT.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace Gadgeteer.ServiceBusDirect
{
    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {

            ethernet_J11D.UseThisNetworkInterface();
            //ethernet_J11D.UseDHCP();

            string[] dns;
            dns = new string[1] { "4.2.2.3" };

           
            ethernet_J11D.NetworkSettings.EnableStaticIP("206.205.188.35", "255.255.252.0", "206.205.188.1");
            ethernet_J11D.NetworkSettings.EnableStaticDns(dns);
            
            //Thread.Sleep(10000);

            //var interfaces = NetworkInterface.GetAllNetworkInterfaces();


            var networkInterface = NetworkInterface.GetAllNetworkInterfaces()[0];

            Debug.Print("Interface set to " + networkInterface.IPAddress);
            Debug.Print(DateTime.Now.ToString());

            Microsoft.SPOT.Hardware.Utility.SetLocalTime(Microsoft.ServiceBus.Micro.NtpClient.GetNetworkTime());
            
            byte[] ca = Gadgeteer.ServiceBusDirect.Resource1.GetBytes(
                Gadgeteer.ServiceBusDirect.Resource1.BinaryResources.VerisignCA);

            X509Certificate[] caCerts =
                new X509Certificate[] { new X509Certificate(ca) };

            multicolorLed.TurnOff();

            Timer timer = new Timer(3000, Timer.BehaviorType.RunContinuously);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(Timer timer)
        {
           var command = HttpClient.ReadAndDeleteFromQueue(new Uri("https://[YOUR NAMESPACE].servicebus.Windows.net/[QUEUE]/messages/head?timeout=5"), null);

            if(command !=null)
            {

                switch (command)
                {
                    case "red":
                        multicolorLed.TurnRed();
                        break;
                    case "blue":
                        multicolorLed.TurnBlue();
                        break;
                    case "green":
                        multicolorLed.TurnGreen();
                        break;
                    case "white":
                        multicolorLed.TurnWhite();
                        break;
                    default:
                        multicolorLed.BlinkRepeatedly(Color.Red);
                        break;
                }
           }
        }
    }
}
