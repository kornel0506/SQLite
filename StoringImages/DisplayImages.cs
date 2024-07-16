using StoringImages.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoringImages
{
    public partial class DisplayImages : Form
    {
        private ImageHelper imageHelper = new ImageHelper();

        public DisplayImages()
        {
            InitializeComponent();
        }

        private void DisplayImages_Load(object sender, EventArgs e)
        {
            LoadImages();
        }

        private void LoadImages()
        {
            string connectionString = dBFunctions.ConnectionStringSQLite;
            string commandText = "SELECT * FROM ImageStore";
            dBHelper helper = new dBHelper(connectionString);
            if (helper.Load(commandText, "ImageStore"))
            {
                dataGridView1.DataSource = helper.DataSet.Tables[0];
            }
        }

        private void insertImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int imageID = imageHelper.InsertImage();
            if (imageID > 0)
            {
                LoadImages();
            }
            else
            {
                MessageBox.Show("Failed to insert image.");
            }
        }

        private void deleteImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int imageID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ImageStore_Id"].Value);
                imageHelper.DeleteImage(imageID);
                LoadImages();
            }
        }

        private void saveAsImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int imageID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ImageStore_Id"].Value);
                imageHelper.SaveAsImage(imageID);
            }
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int imageID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ImageStore_Id"].Value);
                    string connectionString = dBFunctions.ConnectionStringSQLite;
                    string commandText = "SELECT ImageBlob FROM ImageStore WHERE ImageStore_Id=" + imageID;
                    dBHelper helper = new dBHelper(connectionString);
                    if (helper.Load(commandText))
                    {
                        if (helper.DataSet.Tables[0].Rows.Count == 1)
                        {
                            byte[] imageBlob = (byte[])helper.DataSet.Tables[0].Rows[0]["ImageBlob"];
                            if (imageBlob != null && imageBlob.Length > 0)
                            {
                                try
                                {
                                    using (var ms = new System.IO.MemoryStream(imageBlob))
                                    {
                                        pictureBox1.Image = System.Drawing.Image.FromStream(ms);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Hiba történt a kép betöltésekor: " + ex.Message);
                                }
                            }
                            else
                            {
                                pictureBox1.Image = null;
                                MessageBox.Show("A kiválasztott kép adatai üresek.");
                            }
                        }
                        else
                        {
                            pictureBox1.Image = null;
                            MessageBox.Show("Nem található a kiválasztott kép.");
                        }
                    }
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
        }
    }
}
