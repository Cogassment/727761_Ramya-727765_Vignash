using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace RlgBilling.Models
{
    public class DBConnection
    {
        public SqlConnection con()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Constring"].ConnectionString);
            return connection;
        }
    }
}