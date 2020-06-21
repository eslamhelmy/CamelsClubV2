using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services.Helpers
{
   public class FileHelper
    {
        
        private const string _TEMP_PATH = "uploads/temp";
        public static bool MoveFileFromTempPathToAnotherFolder(string fileName,string newPath)
        {
            try
            {
                var path = !Directory.Exists(HttpContext.Current.Server.MapPath($"~/{newPath}"));
                if (!Directory.Exists(HttpContext.Current.Server.MapPath($"~/{newPath}")))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath($"~/uploads/{newPath}"));

                    if (!string.IsNullOrEmpty(fileName) &&File.Exists(HttpContext.Current.Server.MapPath($"~/{_TEMP_PATH}/{fileName}")))
                {
                    File.Move(HttpContext.Current.Server.MapPath($"~/{_TEMP_PATH}/{fileName}"), HttpContext.Current.Server.MapPath($"~/uploads/{newPath}/{fileName}"));
                    return true;
                }
                return false;
                 
            }
            catch(Exception ex)
            {
            }
            return false;
        }

        public static bool MoveDirectoryFromTempPathToAnotherFolder(string folderName, string newPath)
        {
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath($"~/{newPath}")))
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath($"~/uploads/{newPath}"));

                if (!string.IsNullOrEmpty(folderName) && Directory.Exists(HttpContext.Current.Server.MapPath($"~/{_TEMP_PATH}/{folderName}")))
                {
                    Directory.Move(HttpContext.Current.Server.MapPath($"~/{_TEMP_PATH}/{folderName}"), HttpContext.Current.Server.MapPath($"~/uploads/{newPath}/{folderName}"));
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public static string GetFullPath(string filePath)
        {
            string protocol = HttpContext.Current.Request.Url.Scheme.ToString();
            string hostName = HttpContext.Current.Request.Url.Authority.ToString();

            return protocol + "://" + hostName+$"/{filePath}";
        }

    }
}
