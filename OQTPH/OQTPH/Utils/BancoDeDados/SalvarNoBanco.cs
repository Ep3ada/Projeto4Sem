using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace OQTPH.Utils.BancoDeDados
{
    public static class SalvarNoBanco
    {
        public static void Salvar(SqlCommand cmd)
        {
            using (SqlConnection conn = Sql.OpenConnection())
            {
                
            }
        }
    }
}