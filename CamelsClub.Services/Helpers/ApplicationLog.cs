using CamelsClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CamelsClub.Services.Helpers
{
    public static class ApplicationLogService //: IApplicationLogService
    {
        public static bool AddApplicationLog(ApplicationLogCreateViewModel log)
        {
            SqlConnection conn;
            conn = new SqlConnection("Data Source=camels-club-v2.pivotrs.com;Initial Catalog=camels-club-v2;Persist Security Info=True;User ID=camels-club-v2;Password=F#1#top@@;MultipleActiveResultSets=True;");
            conn.Open();
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"AddExceptionLog";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter p1 = new SqlParameter("@LogTypeID", log.LogTypeID);
                SqlParameter p2 = new SqlParameter("@Title", log.Title);
                SqlParameter p3 = new SqlParameter("@Description", log.Description);
                SqlParameter p4 = new SqlParameter("@Data", log.Data);
                SqlParameter p5 = new SqlParameter("@IP", log.IP);
                SqlParameter p6 = new SqlParameter("@URL", log.URL);
                SqlParameter p7 = new SqlParameter("@CreatedDate", DateTime.Now);

                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);
                cmd.Parameters.Add(p5);
                cmd.Parameters.Add(p6);
                cmd.Parameters.Add(p7);

                try
                {
                    int res = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
                return true;
            }


        }
        public static void NotifyMe(ApplicationLogCreateViewModel log)
        {
            //string url = WebConfigurationManager.AppSettings["ApplicationLogViewEmail"] + $"?url={log.URL}&message=Error";
            //WebRequest objRequest = System.Net.HttpWebRequest.Create(url);
            //StreamReader sr = new StreamReader(objRequest.GetResponse().GetResponseStream());
            //var body = sr.ReadToEnd();
            //sr.Close();
            var body = String.Join("--", log.URL,log.Description,log.Data) ;
         //   Task.Run(() =>
         //   {
                EmailHelper.SendMailTest2("test@gmail.com", log.Title, body);
         //   });


        }
    }
}
