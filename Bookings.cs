using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cineverse
{
    public partial class Bookings : UserControl
    {
        public Bookings()
        {
            InitializeComponent();
        }

        private void Bookings_Load(object sender, EventArgs e)
        {

        }

        public void SetBooking(string movieName, string seats, string date, string time, decimal totalPrice)
        {
            label1.Text = movieName;
            label2.Text = seats;
            label3.Text = date;  
            label4.Text = time;
            label5.Text = totalPrice.ToString("0.00") + " BDT";
        }


    }
}
