using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace QuanLyCuaHang.Class
{
    class Connection
    {
        private static string stringConnection = @"Data Source= DESKTOP-CNOJ62V;Initial Catalog = QuanLyCuaHang; Integrated Security=True";
        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(stringConnection);
        }
    }
}
