using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;

namespace TastingsScheduler
{
    class MessageSender
    {
        //public const string API_KEY = "AAAAdqVJxm0:APA91bFVnjrPDCCeaQUyke9Am3tNCD05pXsAuj1PFh1-kFzEI6mTy72KXNmdfCrNTisuWMeXSF5l23ZSdwLPUce8C1a7oq_JJyodNgi8NZevIFNIfkaGFxKIX-imvMeW9ufSVirhpuXb";
        //public const string MESSAGE = "please review about tasted wine";

        public  void SendNotification(string token,string WineName)
        {
            string API_KEY = ConfigurationManager.AppSettings["API_KEY"];
            string MESSAGE = "You have tasted "+ WineName + ". Please share your views with us!";
            var jGcmData = new JObject();
            var jData = new JObject
            {
                { "message", MESSAGE },
                {"wineid","476" }

            };
            //for sending to topic(group)
            //jGcmData.Add("to", "/topics/global");
            //to send to individual
            jGcmData.Add("to", token);
            jGcmData.Add("data", jData);

            var url = new Uri("https://gcm-http.googleapis.com/gcm/send");
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.TryAddWithoutValidation(
                        "Authorization", "key=" + API_KEY);

                    Task.WaitAll(client.PostAsync(url,
                        new StringContent(jGcmData.ToString(), Encoding.Default, "application/json"))
                            .ContinueWith(response =>
                            {
                                Console.WriteLine(response);
                                Console.WriteLine("Message sent: check the client device notification tray.");
                            }));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to send GCM message:");
                Console.Error.WriteLine(e.StackTrace);
            }
        }
    }
}