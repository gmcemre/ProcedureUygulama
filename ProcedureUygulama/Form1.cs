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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Server=. ; Database=Northwind ; Integrated Security=true");
        private void btnEkle_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("UrunEkle", con);
            // "UrunEkle" komutunun Store Procedure olduğunu belirttik.
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
            cmd.Parameters.AddWithValue("@birimFiyat", nudFiyat.Value);
            cmd.Parameters.AddWithValue("@hedefStokDuzeyi", nudStok.Value);

            con.Open();
            try
            {
                int etk = cmd.ExecuteNonQuery();
                con.Close();
                if (etk > 0)
                {
                    MessageBox.Show("Kayıt Eklendi.");
                    UrunListele();
                }
                else
                {
                    MessageBox.Show("Ürün Listede Mevcut.");
                }
            }
            catch (Exception)
            {
                con.Close();
                MessageBox.Show("Kayıt eklenirken hata oluştu.");
            }
        }

        private void UrunListele()
        {
            SqlDataAdapter adp = new SqlDataAdapter("Select * from Urunler where Sonlandi=0", con);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UrunListele();
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                SqlCommand cmd = new SqlCommand("UrunSil", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells["UrunID"].Value);
                con.Open();
                try
                {
                    int etk = cmd.ExecuteNonQuery();
                    con.Close();
                    if (etk > 0)
                    {
                        MessageBox.Show($"{etk} row affected \n Kayıt Silindi.");
                        UrunListele();
                    }
                    else
                    {
                        MessageBox.Show("(0) row affected");
                    }
                }
                catch (Exception ex)
                {
                    con.Close();
                    MessageBox.Show("Kayıt Silinirken Bir Hata Oluştu." + ex.Message);
                }
            }
        }

        private void btnKategoriListe_Click(object sender, EventArgs e)
        {
            KategoriForm kf = new KategoriForm();
            kf.ShowDialog();
        }
    }
}
