using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Data.Extentions
{
    public static class DbContextExtension
    {
        public static List<T> ExecuteStored<T>(this DbContext context, string storedName, params object[] parameters)
        {
            string command = string.Join(",", parameters.Select(p => (p is DateTime || p is string) ? $"'{p}'" : p).ToList());

            return context.Database.SqlQuery<T>($"{storedName} {command}").AsEnumerable().ToList<T>();
        }

        public static T ExecuteScalarFunction<T>(this DbContext context, string functionName, params object[] parameters)
        {
            string command = string.Join(",", parameters.Select(p => (p is DateTime || p is string) ? $"'{p}'" : p).ToList());

            return context.Database.SqlQuery<T>($"select {functionName}({command})").FirstOrDefault();
        }

        public static bool ExecuteNonQuery(this DbContext context, string storedName, params object[] parameters)
        {
            string command = string.Join(",", parameters);

            var affectedRows = context.Database.ExecuteSqlCommand($"{storedName} {command}");

            return affectedRows > 0;
        }

    }
}
