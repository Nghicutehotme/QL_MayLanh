using QuanLyCuaHang.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyCuaHang
{

    public partial class Login : Form
    {
        DataTable tbl_tk;
        public Login()
        {
            InitializeComponent();

        }
        private void Login_Load(object sender, EventArgs e)
        {
            Functions.Connect();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
          
            string sql = "select * from TaiKhoan where TenDangNhap=N'" + txtUser.Text + "'and MatKhau =N'" + txtPassword.Text + "'";
            tbl_tk = Functions.GetDataTable(sql);
            if (tbl_tk.Rows.Count > 0)
            {
                MessageBox.Show("Đăng nhập thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                frmMainEdit main = new frmMainEdit(tbl_tk.Rows[0][0].ToString(), tbl_tk.Rows[0][1].ToString(), tbl_tk.Rows[0][2].ToString(), tbl_tk.Rows[0][3].ToString());
                main.Show();
            }
            else
            {
                MessageBox.Show("Vui lòng điền thông tin đăng nhập");
            }
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            txtUser.Clear();
            txtPassword.Clear();
            txtUser.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}
