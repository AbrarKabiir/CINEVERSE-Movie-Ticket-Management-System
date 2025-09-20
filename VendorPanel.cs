using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cineverse
{
    public partial class VendorPanel : Form
    {
        public VendorPanel()
        {
            InitializeComponent();
            panel2.Visible = false;
            panel3.Visible = false;

           
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void SellerPanel_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;

            panel2.Refresh();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel2.Visible = false;

            panel3.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
