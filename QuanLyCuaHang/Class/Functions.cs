using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;
using System.Drawing;

namespace QuanLyCuaHang.Class
{
    internal class Functions
    {
        public static SqlConnection con;
        public static void Connect()
        {
            con = new SqlConnection();
            con.ConnectionString = ("Data Source= DESKTOP-CNOJ62V;Initial Catalog = QuanLyCuaHang; Integrated Security=True");
            if (con.State != ConnectionState.Open)
            {
                con.Open();
                MessageBox.Show("Kết nối database thành công");
            }
            else
                MessageBox.Show("Kết nối database thất bại");

        }
        // Dừng việc kết nối đến database
        public static void Disconnect()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();
                con = null;
            }
        }
        // phương thức thực thi câu lệnh select dữ liệu
        public static DataTable GetDataTable(string str)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(str, con);
            adt.Fill(dt);
            return dt;
        }
        // phương thức thực thi insert, update, delete

        public static void RunSQL(string str)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con; 
            cmd.CommandText = str;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            cmd.Dispose();
            cmd = null;

        }
        public static bool CheckKey(string str)
        {
            SqlDataAdapter ad = new SqlDataAdapter(str, con);
            DataTable table = new DataTable();
            ad.Fill(table);
            if (table.Rows.Count > 0)
            {
                return true;// có tồn tại khóa ròi
            }
            return false;    // chưa tồn tại khóa
        }
        public static void fillCombo(string sql, ComboBox cbo, string ma, string ten)
        {
            SqlDataAdapter dap = new SqlDataAdapter(sql, con);
            DataTable table = new DataTable();
            dap.Fill(table);
            cbo.DataSource = table;
            cbo.ValueMember = ma;
            cbo.DisplayMember = ten; 
        }

        public static string GetFieldValues(string sql)
        {
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            return ma;
        }
        public static string CreateKey(string tiento)
        {
            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            string d = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }
            public static string[] mNumText = "không;một;hai;ba;bốn;năm;sáu;bảy;tám;chín".Split(';');
            private static string DocHangChuc(double so, bool daydu)
            {
                string chuoi = "";
                Int64 chuc = Convert.ToInt64(Math.Floor((double)(so / 10)));
                Int64 donvi = (Int64)so % 10;
                if (chuc > 1)
                {
                    chuoi = " " + mNumText[chuc] + " mươi";
                    if (donvi == 1)
                    {
                        chuoi += " mốt";
                    }
                }
                else if (chuc == 1)
                {
                    chuoi = " mười";
                    if (donvi == 1)
                    {
                        chuoi += " một";
                    }
                }
                else if (daydu && donvi > 0)
                {
                    chuoi = " lẻ";
                }
                if (donvi == 5 && chuc >= 1)
                {
                    chuoi += " lăm";
                }
                else if (donvi > 1 || (donvi == 1 && chuc == 0))
                {
                    chuoi += " " + mNumText[donvi];
                }
                return chuoi;
            }
            private static string DocHangTram(double so, bool daydu)
            {
                string chuoi = "";
                Int64 tram = Convert.ToInt64(Math.Floor((double)so / 100));
                so = so % 100;
                if (daydu || tram > 0)
                {
                    chuoi = " " + mNumText[tram] + " trăm";
                    chuoi += DocHangChuc(so, true);
                }
                else
                {
                    chuoi = DocHangChuc(so, false);
                }
                return chuoi;
            }
            private static string DocHangTrieu(double so, bool daydu)
            {
                string chuoi = "";
                //Lấy số hàng triệu
                Int64 trieu = Convert.ToInt64(Math.Floor((double)so / 1000000));
                //Lấy phần dư sau số hàng triệu ví dụ 2,123,000 => so = 123,000
                so = so % 1000000;
                if (trieu > 0)
                {
                    chuoi = DocHangTram(trieu, daydu) + " triệu";
                    daydu = true;
                }
                //Lấy số hàng nghìn
                Int64 nghin = Convert.ToInt64(Math.Floor((double)so / 1000));
                //Lấy phần dư sau số hàng nghin 
                so = so % 1000;
                if (nghin > 0)
                {
                    chuoi += DocHangTram(nghin, daydu) + " nghìn";
                    daydu = true;
                }
                if (so > 0)
                {
                    chuoi += DocHangTram(so, daydu);
                }
                return chuoi;
            }
            public static string ChuyenSoSangChuoi(double so)
            {
                if (so == 0)
                    return mNumText[0];
                string chuoi = "", hauto = "";
                Int64 ty;
                do
                {
                    //Lấy số hàng tỷ
                    ty = Convert.ToInt64(Math.Floor((double)so / 1000000000));
                    //Lấy phần dư sau số hàng tỷ
                    so = so % 1000000000;
                    if (ty > 0)
                    {
                        chuoi = DocHangTrieu(so, true) + hauto + chuoi;
                    }
                    else
                    {
                        chuoi = DocHangTrieu(so, false) + hauto + chuoi;
                    }
                    hauto = " tỷ";
                } while (ty > 0);
                return chuoi + " đồng";
            }
   
        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }
    }
   
}
