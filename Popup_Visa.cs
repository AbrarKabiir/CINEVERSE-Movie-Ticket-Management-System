using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Cineverse
{
    public partial class Popup_Visa : Form
    {
        private TicketInfo ticket;
        public Popup_Visa(TicketInfo t)
        {
            InitializeComponent();
            ticket = t;
        }

        private void Popup_Visa_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnString = "Data Source=ABRAR;Initial Catalog=CINEVERSE;Integrated Security=True;TrustServerCertificate=True";

            SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO BOOKING_HISTORY (MV_NAME, DATE, TIME, THEATRE, SEATS) " + "VALUES (@MV_NAME, @DATE, @TIME, @THEATRE, @SEATS)", conn);
            cmd.Parameters.AddWithValue("@MV_NAME", ticket.Movie);
            cmd.Parameters.AddWithValue("@DATE", ticket.Date);
            cmd.Parameters.AddWithValue("@TIME", ticket.Time);
            cmd.Parameters.AddWithValue("@THEATRE", ticket.Theatre);
            cmd.Parameters.AddWithValue("@SEATS", string.Join(", ", ticket.Seats));

            cmd.ExecuteNonQuery();
            conn.Close();


            ThankYou ty = new ThankYou(ticket);
            ty.Show();
            this.Hide();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
