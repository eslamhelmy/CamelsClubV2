using CamelsClub.API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.Script.Serialization;

namespace CamelsClub.API.Helpers
{
    public class APIHelper
    {
        public static List<ValidationMessage> ValidationMessages(ModelStateDictionary keyValues)
        {
            var errors = new List<ValidationMessage>();
            foreach (var state in keyValues)
            {
                var key = state.Key;
                if (key.Contains("."))
                {
                    int index = key.IndexOf(".");
                    key = key.Substring(index + 1);
                }
                foreach (var item in state.Value.Errors)
                {
                    ValidationMessage validationMessage = new ValidationMessage();
                    validationMessage.Key = key;
                    validationMessage.Message = item.ErrorMessage;
                    errors.Add(validationMessage);
                }
            }
            return errors;
        }

        public static bool SendMail(string ToEmail, string EmailSubject, string EmailBody)
        {
            try
            {
                string senderMail = ConfigurationManager.AppSettings["SenderMail"].ToString();
                string senderPassword = ConfigurationManager.AppSettings["SenderPassword"].ToString();
                SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["HostName"].ToString(), int.Parse(ConfigurationManager.AppSettings["PortNumber"].ToString()));
                var host = client.Host;
                if (host == "smtp.gmail.com")
                    client.EnableSsl = true;
                else
                    client.EnableSsl = false;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderMail, senderPassword);
                MailMessage mailMessage = new MailMessage(senderMail, ToEmail, EmailSubject, EmailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static string SendNotification(string DeviceToken, List<string> DeviceTokenList, string title, string msg, string type)
        {
            if (DeviceToken == null || DeviceToken == "")
            {
                DeviceToken = "/topics/all";
            }
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAgjWE6hc:APA91bFWwTLitPEYkc-G8ciJu-Vjb-0v2YEZ2ThI1D5U6pQU84YSyckbAUFjA-U0Hwqik32H9Nyn3tkXy1OzPNMWtaaGNbDmkPxaeO_QgTJZaq_BdR_z16raLyDrd9qblQbiOCyHMdxX"));
            httpWebRequest.Headers.Add(string.Format("Sender: id={0}", "559243651607"));
            //httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAAS6ZVEv8:APA91bH2s3JnsZXO2rFJBMWvIyW_Gl5VzJx4QJ0HbG43fQrTga7RPnW5ZdPYmAgleuNCK82lfTHedw4MKxjeA0X8ccbxWYUhh3xqjgrfzLO5FcDEazaRx54cJtU0qiOgPmYE-Iq5NEP8"));
            //httpWebRequest.Headers.Add(string.Format("Sender: id={0}", "324913140479"));
            httpWebRequest.Method = "POST";
            var payload = new Object();
            var diviceTokensCount = DeviceTokenList.Count;
            if (diviceTokensCount == 0)
            {
                payload = new
                {
                    to = DeviceToken,
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        type,
                        body = msg,
                        title = title
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
                        body = msg,
                        title = title
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

    }
}