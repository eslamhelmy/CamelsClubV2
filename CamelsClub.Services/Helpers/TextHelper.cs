using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace CamelsClub.Services.Helpers
{
    public static class TextHelper
    {
        public static string RemoveHtmlTags(this string htmlText)
        {
            return Regex.Replace(htmlText, "<(.|\\n)*?>", string.Empty);
        }

        public static string GenerateURL(this string Title)
        {
            string text = Title.ToString();
            text = text.Trim();
            text = text.Trim('-');
            text = text.ToLower();
            char[] array = "$%#@!*?;:~`+=()[]{}|\\'<>,/^&\".".ToCharArray();
            text = text.Replace("c#", "C-Sharp");
            text = text.Replace("vb.net", "VB-Net");
            text = text.Replace("asp.net", "Asp-Net");
            text = text.Replace(".", "-");
            for (int i = 0; i < array.Length; i++)
            {
                string text2 = array.GetValue(i).ToString();
                if (text.Contains(text2))
                {
                    text = text.Replace(text2, string.Empty);
                }
            }
            text = text.Replace(" ", "-");
            text = text.Replace("--", "-");
            text = text.Replace("---", "-");
            text = text.Replace("----", "-");
            text = text.Replace("-----", "-");
            text = text.Replace("----", "-");
            text = text.Replace("---", "-");
            text = text.Replace("--", "-");
            text = text.Trim();
            return text.Trim('-');
        }
        public static string GetRandomCode(int len)
        {
            int maxSize = len;
            char[] chars = new char[30];
            string a;
            a = "1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data) { result.Append(chars[b % (chars.Length)]); }
            return result.ToString();
        }

        public static double? GetDrivingDistanceInMiles(string origin, string destination)
        {
            string url = @"https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" +
              origin + "&destinations=" + destination +
              "&mode=driving&sensor=false&language=en-EN&units=metric&key=AIzaSyBgYkW2sy4okUchNj3blkvcvl_7ChZbZ4Q";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);

            List<double> Distances = new List<double>();
            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                if (distance.Count == 0)
                    return null;
                else
                {                 
                    return distance[0].ChildNodes[1].InnerText.Contains("km") ? Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" km", "").ToString()) : Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" m", "").ToString()) / 1000;
                }         
            }
            return null;
        }
    }
}
