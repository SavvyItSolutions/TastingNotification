using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using NLog;
using System.Configuration;
using System.IO;

namespace TastingsScheduler
{
    public class MessageSenderIOS
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void sendMessage(string BarCode,string token,string WineName,int StoreId)
        {
            try
            {
                var succeeded = 0;
                var failed = 0;
                var attempted = 0;
                logger.Info("Sending notification for :" + BarCode);
                //var config = new ApnsConfiguration (ApnsConfiguration.ApnsServerEnvironment.Sandbox, Settings.Instance.ApnsCertificateFile, Settings.Instance.ApnsCertificatePassword);
                var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production, "Production2.p12", "Wineoutlet@99666");
                var broker = new ApnsServiceBroker(config);
                broker.ChangeScale(10);
                broker.OnNotificationFailed += (notification, exception) =>
                {
                    failed++;
                };
                broker.OnNotificationSucceeded += (notification) =>
                {
                    succeeded++;
                };
                broker.Start();
                string dt = token;       //"3b0d4407d7fdfb5b3c4c0f421004407d5787595b4dea0875a75db9de75d368a0";
                broker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = dt,
                    //Payload = JObject.Parse("{\"aps\":{ \"alert\" : \"You've just tasted a new wine\" },{\"title\":\"222\"}")
                    //Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"You've just tasted a new wine\" } }")
                    Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"You've just tasted " + WineName + ". Please review the wine.\" },\"barcode\":\"" + BarCode + "\",\"storeid\":\"" + StoreId + "\" }")
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
            catch(Exception ex)
            {
                string path = ConfigurationManager.AppSettings["ErrorLog"];
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                System.IO.Directory.CreateDirectory(path);
                using (StreamWriter writer = new StreamWriter(path + "Error.txt", true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
        }
    }
}
