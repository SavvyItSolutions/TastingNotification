using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingIT
{
    class Program
    {
        static void Main(string[] args)
        {
            something();
        }

        static void something()
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            //var config = new ApnsConfiguration (ApnsConfiguration.ApnsServerEnvironment.Sandbox, Settings.Instance.ApnsCertificateFile, Settings.Instance.ApnsCertificatePassword);
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, "PushNotificationCertificate.p12", "Wineoutlet@99666");
            var broker = new ApnsServiceBroker(config);
            broker.OnNotificationFailed += (notification, exception) => {
                failed++;
            };
            broker.OnNotificationSucceeded += (notification) => {
                succeeded++;
            };
            broker.Start();
            string dt = "3b0d4407d7fdfb5b3c4c0f421004407d5787595b4dea0875a75db9de75d368a0";
            broker.QueueNotification(new ApnsNotification
            {
                DeviceToken = dt,
                //Payload = JObject.Parse("{\"aps\":{ \"alert\" : \"You've just tasted a new wine\" },{\"title\":\"222\"}")
                //Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"You've just tasted a new wine\" } }")
                Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"You've just tasted a new wine\" },\"wineid\":\"95\" }")
            });

            //foreach (var dt in Settings.Instance.ApnsDeviceTokens)
            //{
            //    attempted++;
            //    broker.QueueNotification(new ApnsNotification
            //    {
            //        DeviceToken = dt,
            //        Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"Hello PushSharp!\" } }")
            //    });
            //}

            broker.Stop();

            //Assert.AreEqual(attempted, succeeded);
            //Assert.AreEqual(0, failed);
        }
    }
}
