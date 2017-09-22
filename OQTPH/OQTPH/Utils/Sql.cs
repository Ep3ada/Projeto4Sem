using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace OQTPH.Utils
{
    public static class Sql
    {
        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection("Server=tcp:ep3serv.database.windows.net,1433;Initial Catalog=EPBD;Persist Security Info=False;User ID=project-master@ep3serv;Password=Band2017;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            conn.Open();

            return conn;
        }
    }
}