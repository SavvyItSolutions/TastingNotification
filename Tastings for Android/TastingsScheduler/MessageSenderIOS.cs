﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using NLog;

namespace TastingsScheduler
{
    public class MessageSenderIOS
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void sendMessage(int wineId,string token,string WineName)
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;
            logger.Info("Sending notification for :"+wineId);
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
            string dt = token;       //"3b0d4407d7fdfb5b3c4c0f421004407d5787595b4dea0875a75db9de75d368a0";
            broker.QueueNotification(new ApnsNotification
            {
                DeviceToken = dt,
                //Payload = JObject.Parse("{\"aps\":{ \"alert\" : \"You've just tasted a new wine\" },{\"title\":\"222\"}")
                //Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"You've just tasted a new wine\" } }")
                Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"You've just tasted "+WineName+". Please review the wine.\" },\"wineid\":\""+wineId+"\" }")
            });
            logger.Info("Notification sent");
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
        }
    }
}