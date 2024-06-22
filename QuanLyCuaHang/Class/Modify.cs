using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace QuanLyCuaHang.Class
{
    class Modify
    {
        SqlDataAdapter dataAdapter;
        SqlCommand command;

        public Modify()
        { }
        public DataTable getCTHD()
        {
            DataTable dt = new DataTable();
            string sql = "select *from ChiTietHDBan";
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                dataAdapter = new SqlDataAdapter(sql, sqlConnection);
                dataAdapter.Fill(dt);
                sqlConnection.Close();
            }
            return dt;
        }

        public DataTable getKho()
        {
            DataTable dt = new DataTable();
            string sql = "select *from Kho";
            using (SqlConnection sqlConnection = Connection.GetSqlConnection())
            {
                sqlConnection.Open();
                dataAdapter = new SqlDataAdapter(sql, sqlConnection);
                dataAdapter.Fill(dt);
                sqlConnection.Close();
            }
            return dt;
        }
    }
}
