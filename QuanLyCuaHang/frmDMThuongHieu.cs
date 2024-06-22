using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyCuaHang.Class;
using System.Data.SqlClient;

namespace QuanLyCuaHang
{
    public partial class frmDMThuongHieu : Form
    {
        DataTable tblCL;

        public frmDMThuongHieu()
        {
            InitializeComponent();

        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT MaNCC, TenNCC FROM CungCap";
            tblCL = Functions.GetDataTable(sql); //Đọc dữ liệu từ bảng
            dgv_ncc.DataSource = tblCL; //Nguồn dữ liệu            
            dgv_ncc.Columns[0].HeaderText = "Mã nhà cung cấp";
            dgv_ncc.Columns[1].HeaderText = "Tên nhà cung cấp";
            dgv_ncc.Columns[0].Width = 100;
            dgv_ncc.Columns[1].Width = 300;
            dgv_ncc.AllowUserToAddRows = false; //Không cho người dùng thêm dữ liệu trực tiếp
            dgv_ncc.EditMode = DataGridViewEditMode.EditProgrammatically; //Không cho sửa dữ liệu trực tiếp
        }

        private void dgv_ncc_Click(object sender, EventArgs e)
        {
            if (btnThem.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNCC.Focus();
                return;
            }

            if (tblCL.Rows.Count == 0)// không có dòng dữ liệu được chọn
            {
                MessageBox.Show("Không có dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int i;
            i = dgv_ncc.CurrentRow.Index;
            txtMaNCC.Text = dgv_ncc.Rows[i].Cells[0].Value.ToString();
            txtTenNCC.Text = dgv_ncc.Rows[i].Cells[1].Value.ToString();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnBoQua.Enabled = true;
        }

        public void Reset()
        {
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
        }

        private void txtMaNCC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void frmDMThuongHieu_Load(object sender, EventArgs e)
        {
            txtMaNCC.Enabled = false;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            LoadDataGridView();
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnThem.Enabled = false;
            btnBoQua.Enabled = true;
            btnLuu.Enabled = true;
            btnDong.Enabled = true;
            Reset();// xóa các textbox
            txtMaNCC.Enabled = true; // cho nhập mới
            txtMaNCC.Focus();
        }

        private void btnLuu_Click_1(object sender, EventArgs e)
        {
            string sql; // chuỗi thực thi kết nối

            if (txtMaNCC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập vào mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNCC.Focus();
                return;
            }
            if (txtTenNCC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập vào tên nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtTenNCC.Focus();
                return;
            }
            sql = "select MaNCC from CungCap where mancc ='" + txtMaNCC.Text + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã nhà cung cấp này đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMaNCC.Focus();
                return;
            }

            sql = "insert into CungCap values(N'" + txtMaNCC.Text + "',N'" + txtTenNCC.Text + "')";
            Functions.RunSQL(sql);// thực hiện câu lệnh sql
            LoadDataGridView();// cập nhật lại
            Reset(); // xóa các textbox
            btnXoa.Enabled = true;
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            btnBoQua.Enabled = false;
            txtMaNCC.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tblCL.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNCC.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "delete CungCap where mancc=N'" + txtMaNCC.Text + "'";
                Functions.RunSQL(sql);
                LoadDataGridView();
                Reset();
            }
        }

        private void btnBoQua_Click(object sender, EventArgs e)
        {
            Reset();
            btnBoQua.Enabled = false;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnLuu.Enabled = false;
            txtMaNCC.Enabled = false;
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql; //Lưu câu lệnh sql
            if (tblCL.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtMaNCC.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txtTenNCC.Text.Trim().Length == 0) //nếu chưa nhập tên chất liệu
            {
                MessageBox.Show("Bạn chưa nhập tên chất liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sql = "update CungCap set tenchatlieu=N'" +
                txtTenNCC.Text.ToString() +
                "' where mancc=N'" + txtMaNCC.Text + "'";
            Functions.RunSQL(sql);
            LoadDataGridView();
            Reset();
            btnBoQua.Enabled = false;
        }

        private void dgv_ncc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
