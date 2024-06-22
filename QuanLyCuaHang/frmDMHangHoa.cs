using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyCuaHang.Class;

namespace QuanLyCuaHang
{
    public partial class frmDMHangHoa : Form
    {
        DataTable tbl_hh;
        public frmDMHangHoa()
        {
            InitializeComponent();
        }
        private void frmDMHangHoa_Load(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT * from CungCap";
            txt_mahang.Enabled = false;
            btn_luu.Enabled = false;
            btn_boqua.Enabled = false;
            LoadDataGridView();
            Functions.fillCombo(sql, cbo_mancc, "MaNCC", "TenNCC");
            cbo_mancc.SelectedIndex = -1;
            ResetValues();
        }

        private void ResetValues()
        {
            txt_mahang.Text = "";
            txt_tenhang.Text = "";
            cbo_mancc.Text = "";
            txt_soluong.Text = "0";
            txt_dongianhap.Text = "0";
            txt_dongiaban.Text = "0";
            txt_soluong.Enabled = true;
            txt_dongianhap.Enabled = false;
            txt_dongiaban.Enabled = false;
            txt_ghichu.Text = "";
        }
        private void LoadDataGridView()
        {
            string sql;
            sql = "SELECT * from HangHoa";
            tbl_hh = Functions.GetDataTable(sql);
            dgv_hanghoa.DataSource = tbl_hh;
            dgv_hanghoa.Columns[0].HeaderText = "Mã hàng";
            dgv_hanghoa.Columns[1].HeaderText = "Tên hàng";
            dgv_hanghoa.Columns[2].HeaderText = "Hãng";
            dgv_hanghoa.Columns[3].HeaderText = "Số lượng";
            dgv_hanghoa.Columns[4].HeaderText = "Đơn giá nhập";
            dgv_hanghoa.Columns[5].HeaderText = "Đơn giá bán";
            dgv_hanghoa.Columns[6].HeaderText = "Ghi chú";
            dgv_hanghoa.Columns[7].HeaderText = "Hinh";

            dgv_hanghoa.Columns[0].Width = 80;
            dgv_hanghoa.Columns[1].Width = 140;
            dgv_hanghoa.Columns[2].Width = 80;
            dgv_hanghoa.Columns[3].Width = 80;
            dgv_hanghoa.Columns[4].Width = 100;
            dgv_hanghoa.Columns[5].Width = 100;
            dgv_hanghoa.Columns[6].Width = 300;
            dgv_hanghoa.Columns[7].Width = 80;
            dgv_hanghoa.AllowUserToAddRows = false;
            dgv_hanghoa.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void dgv_hanghoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string MaNCC;
            string sql;
            if (btn_them.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_mahang.Focus();
                return;
            }
            if (tbl_hh.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
           txt_mahang.Text = dgv_hanghoa.CurrentRow.Cells["MaHang"].Value.ToString();
            txt_tenhang.Text = dgv_hanghoa.CurrentRow.Cells["TenHang"].Value.ToString();
            MaNCC = dgv_hanghoa.CurrentRow.Cells["MaNCC"].Value.ToString();
            sql = "SELECT TenNCC FROM CungCap WHERE MaNCC=N'" + MaNCC + "'";
            cbo_mancc.Text = Functions.GetFieldValues(sql);
            txt_soluong.Text = dgv_hanghoa.CurrentRow.Cells["SoLuong"].Value.ToString();
            txt_dongianhap.Text = dgv_hanghoa.CurrentRow.Cells["DonGiaNhap"].Value.ToString();
            txt_dongiaban.Text = dgv_hanghoa.CurrentRow.Cells["DonGiaBan"].Value.ToString();
            sql = "SELECT Ghichu FROM HangHoa WHERE MaHang = N'" + txt_mahang.Text + "'";
            txt_ghichu.Text = Functions.GetFieldValues(sql);
            sql = "SELECT Hinh FROM HangHoa WHERE MaHang = N'" + textAnh.Text + "'";
            textAnh.Text = Functions.GetFieldValues(sql);
            //picAnh.Image = Image.FromFile(textAnh.Text);
            btn_sua.Enabled = true;
            btn_xoa.Enabled = true;
            btn_boqua.Enabled = true;
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            btn_sua.Enabled = false;
            btn_xoa.Enabled = false;
            btn_boqua.Enabled = true;
            btn_luu.Enabled = true;
            btn_them.Enabled = false;
            ResetValues();
            txt_mahang.Enabled = true;
            txt_mahang.Focus();
            txt_soluong.Enabled = true;
            txt_dongianhap.Enabled = true;
            txt_dongiaban.Enabled = true;
        }

        private void btn_luu_Click(object sender, EventArgs e)
        {
            string sql;
            if (txt_mahang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_mahang.Focus();
                return;
            }
            if (txt_tenhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_tenhang.Focus();
                return;
            }
            if (cbo_mancc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_mancc.Focus();
                return;
            }
            if (textAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải chọn ảnh minh họa cho hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Focus();
                return;
            }
            sql = "SELECT MaHang FROM HangHoa WHERE MaHang=N'" + txt_mahang.Text.Trim() + "'";
            if (Functions.CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã tồn tại, bạn phải chọn mã hàng khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_mahang.Focus();
                return;
            }
            sql = "INSERT INTO HangHoa(MaHang,TenHang,MaNCC,SoLuong,DonGiaNhap, DonGiaBan, Ghichu, Hinh) VALUES(N'"
                + txt_mahang.Text.Trim() + "',N'" + txt_tenhang.Text.Trim() +
                "',N'" + cbo_mancc.SelectedValue.ToString() +
                "'," + txt_soluong.Text.Trim() + "," + txt_dongianhap.Text +
                "," + txt_dongiaban.Text + ",N'" + txt_ghichu.Text.Trim() + "', N'" + textAnh.Text.Trim() + "')";

            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btn_xoa.Enabled = true;
            btn_them.Enabled = true;
            btn_sua.Enabled = true;
            btn_boqua.Enabled = false;
            btn_luu.Enabled = false;
            txt_mahang.Enabled = false;
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (tbl_hh.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_mahang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xoá bản ghi này không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sql = "DELETE HangHoa WHERE MaHang=N'" + txt_mahang.Text + "'";
                Functions.RunSQL(sql);
                LoadDataGridView();
                ResetValues();
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            string sql;
            if (tbl_hh.Rows.Count == 0)
            {
                MessageBox.Show("Không còn dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (txt_mahang.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_mahang.Focus();
                return;
            }
            if (txt_tenhang.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập tên hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txt_tenhang.Focus();
                return;
            }
            if (cbo_mancc.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cbo_mancc.Focus();
                return;
            }

            sql = "UPDATE HangHoa SET TenHang=N'" + txt_tenhang.Text.Trim().ToString() +
                "',MaChatLieu=N'" + cbo_mancc.SelectedValue.ToString() +
                "',SoLuong=" + txt_soluong.Text +
                ",Ghichu=N'" + txt_ghichu.Text + "', Hinh ='" + textAnh.Text + "' WHERE MaHang=N'" + txt_mahang.Text + "'";
            Functions.RunSQL(sql);
            LoadDataGridView();
            ResetValues();
            btn_boqua.Enabled = false;
        }

        private void btn_boqua_Click(object sender, EventArgs e)
        {
            ResetValues();
            btn_xoa.Enabled = true;
            btn_sua.Enabled = true;
            btn_them.Enabled = true;
            btn_boqua.Enabled = false;
            btn_luu.Enabled = false;
            txt_mahang.Enabled = false;
        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            string sql;
            if ((txt_mahang.Text == "") && (txt_tenhang.Text == "") && (cbo_mancc.Text == ""))
            {
                MessageBox.Show("Bạn hãy nhập điều kiện tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            sql = "SELECT * from HangHoa WHERE 1=1";
            if (txt_mahang.Text != "")
                sql += " AND MaHang LIKE N'%" + txt_mahang.Text + "%'";
            if (txt_tenhang.Text != "")
                sql += " AND TenHang LIKE N'%" + txt_tenhang.Text + "%'";
            if (cbo_mancc.Text != "")
                sql += " AND MaNCC LIKE N'%" + cbo_mancc.SelectedValue + "%'";
            tbl_hh = Functions.GetDataTable(sql);
            if (tbl_hh.Rows.Count == 0)
                MessageBox.Show("Không có bản ghi thoả mãn điều kiện tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else MessageBox.Show("Có " + tbl_hh.Rows.Count + "  bản ghi thoả mãn điều kiện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgv_hanghoa.DataSource = tbl_hh;
            ResetValues();
        }

        private void btn_hienthids_Click(object sender, EventArgs e)
        {
            string sql;
            sql = "SELECT MaHang,TenHang,MaNCC,SoLuong,DonGiaNhap,DonGiaBan,Ghichu, Hinh FROM HangHoa";
            tbl_hh = Functions.GetDataTable(sql);
            dgv_hanghoa.DataSource = tbl_hh;
        }

        private void btn_dong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                OpenFileDialog dlgOpen = new OpenFileDialog();
                dlgOpen.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
                dlgOpen.FilterIndex = 2;
                dlgOpen.Title = "Chọn ảnh minh hoạ cho sản phẩm";
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    picAnh.Image = Image.FromFile(dlgOpen.FileName);
                    textAnh.Text = dlgOpen.FileName;
                }
        }

    }
}
