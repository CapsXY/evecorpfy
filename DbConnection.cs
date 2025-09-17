using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace evecorpfy.Data
{
    public static class DbConnectionFactory
    {
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DbEveCorpfy"].ConnectionString;
        }

        public static SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection(GetConnectionString());
            conn.Open();
            return conn;
        }
    }
}
