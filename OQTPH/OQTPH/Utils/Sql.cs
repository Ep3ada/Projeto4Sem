using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace OQTPH.Utils
{
    public static class Sql
    {
        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

            conn.Open();

            return conn;
        }
    }
}