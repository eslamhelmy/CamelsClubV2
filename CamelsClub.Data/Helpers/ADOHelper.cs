using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CamelsClub.Data.Context;

namespace CamelsClub.Data.Helpers
{
    public static class ADOHelper
    {
        public static List<T> ExcuteStored<T>(string storedName, Dictionary<string, object> parameters) where T : new()
        {
            using (CamelsClubContext context = new CamelsClubContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                foreach (var param in parameters)
                {
                    parameterList.Add(new SqlParameter("@" + param.Key, param.Value));
                }
                string command = string.Join(",", parameterList.Select(x => x.ParameterName).ToArray());
                if(parameterList.Count==0)
                return context.Database.SqlQuery<T>(storedName).AsEnumerable().ToList<T>();



                return context.Database.SqlQuery<T>(string.Format("{0} {1}", storedName, string.Join(",", parameterList.Select(x => x.ParameterName).ToArray())), parameterList.ToArray()).AsEnumerable().ToList<T>();

            }


        }
        public static DataTable GetDataTable<T>(string storedName, Dictionary<string, object> parameters) where T : new()
        {
            return ExcuteStored<T>(storedName, parameters).ToDataTable<T>();
        }
        public static void ExcuteNonQuery(string storedName, Dictionary<string, object> parameters)
        {
            using (CamelsClubContext context = new CamelsClubContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                foreach (var param in parameters)
                {
                    parameterList.Add(new SqlParameter("@" + param.Key, param.Value));
                }
                string command = string.Join(",", parameterList.Select(x => x.ParameterName).ToArray());
                context.Database.ExecuteSqlCommand(string.Format("{0} {1}", storedName, string.Join(",", parameterList.Select(x => x.ParameterName).ToArray())), parameterList.ToArray());
            }
        }
        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
    }

}
