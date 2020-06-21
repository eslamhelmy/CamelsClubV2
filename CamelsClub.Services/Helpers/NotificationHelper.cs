using CamelsClub.ViewModels;
using CamelsClub.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace CamelsClub.Services.Helpers
{
    public class NotificationHelper
    {
        public static string SendNotification(string DeviceToken, List<string> DeviceTokenList,string userImage,string competitionImage,int? competitionID, string title, string msg, NotificationType type)
        {
            if (DeviceToken == null || DeviceToken == "")
            {
                DeviceToken = "/topics/all";
            }
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAASZtBWxI:APA91bHNilmiVHly2Ojxew73uM6WJO7noYE4hnTqFjWfWKnqLALjJPLJe5KEDTJ1opnc7OOQXYIcTaSDP2SauGa9hVeVSyPNtysCEMWKTB2ggyEeBzZoOUMic9_09yz_4MgJrkOH1_6n"));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", "316137364242"));
            httpWebRequest.Method = "POST";
            var payload = new Object();
            var diviceTokensCount =DeviceTokenList!= null? DeviceTokenList.Count:0;
            if (diviceTokensCount == 0)
            {
                payload = new
                {
                    to = DeviceToken,
                    priority = "high",
                    content_available = true,
                    notification = new {
                         title = title,
                         body= msg
                    },
                   data = new
                    {
                        type,
                        body = new { msg, competitionID, type , competitionImage , userImage },
                        title = title,
                    }
                };
              
            }
            else
            {
                payload = new
                {
                    registration_ids = DeviceTokenList,
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        type,
                        body = new { msg, competitionID, type, competitionImage, userImage },
                        title = title,
                        
                    }
                };
            }
            
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }



        public static string NewSendNotification(string DeviceToken, List<string> DeviceTokenList,NotificationCreateViewModel notificationViewModel)
        {
            if (DeviceToken == null || DeviceToken == "")
            {
                DeviceToken = "/topics/all";
            }
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAASZtBWxI:APA91bHNilmiVHly2Ojxew73uM6WJO7noYE4hnTqFjWfWKnqLALjJPLJe5KEDTJ1opnc7OOQXYIcTaSDP2SauGa9hVeVSyPNtysCEMWKTB2ggyEeBzZoOUMic9_09yz_4MgJrkOH1_6n"));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", "316137364242"));
            httpWebRequest.Method = "POST";
            var payload = new Object();
            var diviceTokensCount = DeviceTokenList != null ? DeviceTokenList.Count : 0;
            if (diviceTokensCount == 0)
            {
                payload = new
                {
                    to = DeviceToken,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = notificationViewModel.ContentArabic,
                        title = notificationViewModel.ContentEnglish
                    },
                 
                    data = notificationViewModel

                };
            }
            else
            {
                payload = new
                {
                    registration_ids = DeviceTokenList,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = notificationViewModel.ContentArabic,
                        title = notificationViewModel.ContentEnglish
                    },

                    data = notificationViewModel
                };
            }
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}
