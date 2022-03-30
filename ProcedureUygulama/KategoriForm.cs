using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcedureUygulama
{
    public partial class KategoriForm : Form
    {
        public KategoriForm()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Server=. ; database = Northwind ; Integrated Security=true");
        private void KategoriForm_Load(object sender, EventArgs e)
        {
            KategoriListele();
        }

        private void KategoriListele()
        {
            SqlDataAdapter adp = new SqlDataAdapter("KategoriListe", con);
            adp.SelectCommand.CommandType = CommandType.StoredProcedure;

            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("KategoriEkle", con);
            cmd.CommandType = CommandType.StoredProcedure;
            if (txtKategoriAdi.Text == "" || txtTanimi.Text == "")
            {
                MessageBox.Show("Lüften boş alanları doldudurunuz.");
            }
            else
            {
                cmd.Parameters.AddWithValue("@kategoriAdi", txtKategoriAdi.Text);
                cmd.Parameters.AddWithValue("@tanimi", txtTanimi.Text);
                con.Open();

                try
                {
                    int etk = cmd.ExecuteNonQuery();
                    con.Close();
                    if (etk > 0)
                    {
                        MessageBox.Show($"{etk} row(s) affected \n Kategori Eklendi.");
                        KategoriListele();
                    }
                    else
                    {
                        MessageBox.Show($"{etk} row(s) affected");
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    MessageBox.Show("Kayıt eklenirken bir hata oluştu." + ex.Message);
                }
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                SqlCommand cmd = new SqlCommand("KategoriSil", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells["KategoriID"].Value);

                con.Open();
                try
                {
                    int etk = cmd.ExecuteNonQuery();
                    con.Close();
                    if (etk > 0)
                    {
                        MessageBox.Show($"{etk} row(s) affected \n Kategori Silindi.");
                        KategoriListele();
                    }
                    else
                    {
                        MessageBox.Show($"{etk} row(s) affected");
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    MessageBox.Show("Kayıt silinirken bir hata oluştu." + ex.Message);
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SqlCommand cmd = new SqlCommand("KategoriGuncelle", con);
            cmd.CommandType = CommandType.StoredProcedure;
            DataGridViewRow row = dataGridView1.CurrentRow;
            cmd.Parameters.AddWithValue("@id", row.Cells["KategoriID"].Value);
            cmd.Parameters.AddWithValue("@adi", row.Cells["KategoriAdi"].Value);
            cmd.Parameters.AddWithValue("@tanimi", row.Cells["Tanimi"].Value);

            con.Open();
            try
            {
                int etk = cmd.ExecuteNonQuery();
                con.Close();
                if (etk > 0)
                {
                    MessageBox.Show($"{etk} row(s) affected \n Kategori Güncellendi.");
                    KategoriListele();
                }
                else
                {
                    MessageBox.Show($"{etk} row(s) affected");
                }
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Kayıt güncellenirken bir hata oluştu." + ex.Message);
            }
        }
    }
}
