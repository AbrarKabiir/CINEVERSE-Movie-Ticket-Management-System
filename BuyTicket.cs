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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Cineverse
{
    public partial class BuyTicket : Form
    {
        private int UserId;
        private string Username;
        List<string> selectedSeats = new List<string>();
        int totalPrice = 0;

        public BuyTicket(int userId, string username)
        {
            InitializeComponent();
            RefreshSeats();
            this.UserId = userId;
            this.Username = username;
        }

        private void BuyTicket_Load(object sender, EventArgs e)
        {
            string dataFile = System.IO.Path.Combine(Application.StartupPath, "AdminData.txt");

            if (System.IO.File.Exists(dataFile))
            {
                string[] lines = System.IO.File.ReadAllLines(dataFile);

                // Movie 1
                if (lines.Length > 0)
                {
                    string[] parts = lines[0].Split('|');
                    if (parts.Length == 2)
                    {
                        BT11.Text = parts[0];
                        if (System.IO.File.Exists(parts[1]))
                            POSTER1.Image = Image.FromFile(parts[1]);
                    }
                }

                // Movie 2
                if (lines.Length > 1)
                {
                    string[] parts = lines[1].Split('|');
                    if (parts.Length == 2)
                    {
                        BT2.Text = parts[0];
                        if (System.IO.File.Exists(parts[1]))
                            POSTER2.Image = Image.FromFile(parts[1]);
                    }
                }

                // Movie 3
                if (lines.Length > 2)
                {
                    string[] parts = lines[2].Split('|');
                    if (parts.Length == 2)
                    {
                        BT3.Text = parts[0];
                        if (System.IO.File.Exists(parts[1]))
                            POSTER3.Image = Image.FromFile(parts[1]);
                    }
                }

                // Movie 4
                if (lines.Length > 3)
                {
                    string[] parts = lines[3].Split('|');
                    if (parts.Length == 2)
                    {
                        BT4.Text = parts[0];
                        if (System.IO.File.Exists(parts[1]))
                            POSTER4.Image = Image.FromFile(parts[1]);
                    }
                }

                // Movie 5
                if (lines.Length > 4)
                {
                    string[] parts = lines[4].Split('|');
                    if (parts.Length == 2)
                    {
                        BT5.Text = parts[0];
                        if (System.IO.File.Exists(parts[1]))
                            POSTER5.Image = Image.FromFile(parts[1]);
                    }
                }
            }
        }

        private void RefreshSeats()
        {
            PANELSEATS.Controls.Clear();
            totalPrice = 0;



            int rows = 4;
            int cols = 7;
            int seatSize = 40; // Button size
            int spacing = 6;   // Space between buttons
            char rowLetter = 'A';

            // Get selected movie, theatre, date, time
            string selectedMovie = "";
            if (BT11.Checked) selectedMovie = BT11.Text;
            else if (BT2.Checked) selectedMovie = BT2.Text;
            else if (BT3.Checked) selectedMovie = BT3.Text;
            else if (BT4.Checked) selectedMovie = BT4.Text;
            else if (BT5.Checked) selectedMovie = BT5.Text;

            string selectedTheatre = COMBOTHEATRE.SelectedItem?.ToString();
            string selectedDate = COMBODATE.SelectedItem?.ToString();
            string selectedTime = COMBOTIME.SelectedItem?.ToString();

            // Get already booked seats
            List<string> bookedSeats = new List<string>();
            if (!string.IsNullOrEmpty(selectedMovie) &&
                !string.IsNullOrEmpty(selectedTheatre) &&
                !string.IsNullOrEmpty(selectedDate) &&
                !string.IsNullOrEmpty(selectedTime))
            {
                bookedSeats = GetBookedSeats(selectedMovie, selectedTheatre, selectedDate, selectedTime);
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Button seatBtn = new Button();
                    seatBtn.Width = seatSize;
                    seatBtn.Height = seatSize;

                    // Positioning buttons
                    seatBtn.Left = j * (seatSize + spacing);
                    seatBtn.Top = i * (seatSize + spacing);

                    // Seat number
                    seatBtn.Text = rowLetter.ToString() + (j + 1).ToString();
                    seatBtn.Name = "btn" + seatBtn.Text;

                    // Check if seat is booked
                    if (bookedSeats.Contains(seatBtn.Text))
                    {
                        seatBtn.BackColor = Color.Red; // Already booked
                        seatBtn.Enabled = false;       // Disable clicking
                    }
                    else
                    {
                        seatBtn.BackColor = Color.Green; // Available
                        seatBtn.Click += SeatBtn_Click;  // Add click event
                    }

                    seatBtn.ForeColor = Color.Black;
                    seatBtn.FlatStyle = FlatStyle.Flat;

                    // Add to panel
                    PANELSEATS.Controls.Add(seatBtn);
                }

                rowLetter++; // Next row letter
            }

        }

        private void SeatBtn_Click(object sender, EventArgs e)
        {
            Button clickedSeat = sender as Button;

            if (clickedSeat.BackColor == Color.Green) // select seat
            {
                clickedSeat.BackColor = Color.Orange;
                selectedSeats.Add(clickedSeat.Text);

                // Add price depending on seat type
                if (radioButton6.Checked) // Premium
                    totalPrice += 500;
                else if (radioButton7.Checked) // Standard
                    totalPrice += 400;
            }
            else if (clickedSeat.BackColor == Color.Orange) // deselect seat
            {
                clickedSeat.BackColor = Color.Green;
                selectedSeats.Remove(clickedSeat.Text);

                // Subtract price depending on seat type
                if (radioButton6.Checked) // Premium
                    totalPrice -= 500;
                else if (radioButton7.Checked) // Standard
                    totalPrice -= 400;
            }

            // Update total price label
            lblTotalPrice.Text = "Total: " + totalPrice + " Tk";

        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                button4.Text = "🗗";
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                button4.Text = "☐";
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            User_Home uh = new User_Home(this.UserId, this.Username);
            uh.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            User_Home uh = new User_Home(this.UserId, this.Username);
            uh.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            User_Review ur = new User_Review(this.UserId, this.Username);
            ur.Show();
            this.Hide();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            User_Review ur = new User_Review(this.UserId, this.Username);
            ur.Show();
            this.Hide();
        }


        public void UpdateRadioButtonText(string text)
        {
            BT11.Text = text;
        }

        public void UpdatePicture(Image image)
        {
            POSTER1.Image = image;
        }

        private void BT11_CheckedChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void BT5_CheckedChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void BT3_CheckedChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void BT2_CheckedChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void BT4_CheckedChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void COMBODATE_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void COMBOTHEATRE_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void COMBOTIME_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSeats.Clear();
            RefreshSeats();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Gather all booking info
            string movieName = "";
            if (BT11.Checked) movieName = BT11.Text;
            else if (BT2.Checked) movieName = BT2.Text;
            else if (BT3.Checked) movieName = BT3.Text;
            else if (BT4.Checked) movieName = BT4.Text;
            else if (BT5.Checked) movieName = BT5.Text;

            string theatre = COMBOTHEATRE.SelectedItem?.ToString();
            string date = COMBODATE.SelectedItem?.ToString();
            string time = COMBOTIME.SelectedItem?.ToString();
            
            if (radioButton6.Checked == false && radioButton7.Checked == false)
            {
                MessageBox.Show("Please select a seat type (Premium or Standard).");
                return;
            }
            string seatType = radioButton6.Checked ? "Premium" : "Standard";

            if (string.IsNullOrEmpty(COMBOTHEATRE.SelectedItem?.ToString()) ||
            string.IsNullOrEmpty(COMBODATE.SelectedItem?.ToString()) ||
            string.IsNullOrEmpty(COMBOTIME.SelectedItem?.ToString()) ||
            string.IsNullOrEmpty(movieName) ||
            string.IsNullOrEmpty(seatType))
            {
                MessageBox.Show("Please select Movie, theatre, Seattype, date, and time.");
                return;
            }

            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Please select at least one seat.");
                return;
            }

            

            // Pass to Payment Form
            User_Pay pf = new User_Pay(movieName, theatre, date, time, seatType, selectedSeats, totalPrice, this.UserId , this.Username);
            pf.Show();
            this.Hide();
        }

        private List<string> GetBookedSeats(string movie, string theatre, string date, string time)
        {
            List<string> bookedSeats = new List<string>();

            // Connection string
            string ConnString = "Data Source=ABRAR;Initial Catalog=CINE_VERSE_DB;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                string query = "SELECT SeatNumbers FROM Booking_History " +
                               "WHERE MovieName = @MovieName AND Theatre = @Theatre " +
                               "AND ShowDate = @ShowDate AND ShowTime = @ShowTime";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MovieName", movie);
                cmd.Parameters.AddWithValue("@Theatre", theatre);
                cmd.Parameters.AddWithValue("@ShowDate", date);
                cmd.Parameters.AddWithValue("@ShowTime", time);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string seats = reader["SeatNumbers"].ToString(); // comma-separated
                    bookedSeats.AddRange(seats.Split(',')); // Add each seat
                }

                conn.Close();
            }

            return bookedSeats;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Upcomings up = new Upcomings(UserId,Username);
            up.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Upcomings up = new Upcomings(UserId, Username);
            up.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Merchandise md = new Merchandise(this.UserId,Username);
            md.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Merchandise md = new Merchandise(this.UserId, Username);
            md.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            User_Booking_History UBH = new User_Booking_History(UserId, Username);
            UBH.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User_Booking_History UBH = new User_Booking_History(UserId, Username);
            UBH.Show();
            this.Hide();
        }
    }
}
