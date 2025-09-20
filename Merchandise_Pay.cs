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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Cineverse
{
    public partial class Merchandise_Pay : Form
    {
        private int totalAmount;
        private int UserId;
        private string Username;

        public Merchandise_Pay(int amount, int userId , string Username)
        {
            InitializeComponent();
            totalAmount = amount;
            UserId = userId;
            this.Username = Username;

            textBox5.ReadOnly = true;
            textBox5.Text = totalAmount.ToString() + " Tk";
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private void Merchandise_Pay_Load(object sender, EventArgs e)
        {
           
        }

        

        private void ProcessPurchase()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill in all required fields",
                                "Missing Information",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return; //stop execution
            }




            string connString = "Data Source=ABRAR;Initial Catalog=CINE_VERSE_DB;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                foreach (DataGridViewRow row in ((Merchandise)this.Owner).dataGridView1.Rows)
                {
                    bool isChecked = row.Cells["SelectItem"].Value is bool selected && selected;

                    if (isChecked && row.Cells["BuyQty"].Value != null)
                    {
                        int buyQty = Convert.ToInt32(row.Cells["BuyQty"].Value);
                        int itemID = Convert.ToInt32(row.Cells["ItemID"].Value);
                        int vendorID = Convert.ToInt32(row.Cells["VendorID"].Value);
                        int price = Convert.ToInt32(row.Cells["Price"].Value.ToString().Replace(" Tk", ""));
                        int totalPrice = buyQty * price;

                        if (buyQty > 0)
                        {
                            // Update stock
                            string updateStock = "UPDATE Items SET Quantity = Quantity - @Qty WHERE ItemID = @ItemID AND VendorID = @VendorID";
                            using (SqlCommand cmd = new SqlCommand(updateStock, conn))
                            {
                                cmd.Parameters.AddWithValue("@Qty", buyQty);
                                cmd.Parameters.AddWithValue("@ItemID", itemID);
                                cmd.Parameters.AddWithValue("@VendorID", vendorID);
                                cmd.ExecuteNonQuery();
                            }

                            // Insert into sales history
                            string insertHistory = @"
                            INSERT INTO SalesHistory (ItemID, QuantitySold, TotalPrice, SaleDate, VendorID,UserID)
                            VALUES (@ItemID, @Qty, @TotalPrice, @SaleDate, @VendorID,@UserID)";
                            
                            using (SqlCommand cmd = new SqlCommand(insertHistory, conn))
                            {
                                cmd.Parameters.AddWithValue("@ItemID", itemID);
                                cmd.Parameters.AddWithValue("@Qty", buyQty);
                                cmd.Parameters.AddWithValue("@TotalPrice", totalPrice);
                                cmd.Parameters.AddWithValue("@SaleDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@VendorID", vendorID);
                                cmd.Parameters.AddWithValue("@UserId", UserId);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                conn.Close();
            }

            MessageBox.Show("Payment successful!");

            // Refresh merchandise grid
            ((Merchandise)this.Owner).LoadAllItems();
            this.Close(); // close payment form
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // Check all required textboxes
            bool textboxesFilled = !string.IsNullOrWhiteSpace(textBox1.Text) &&
                                   !string.IsNullOrWhiteSpace(textBox2.Text) &&
                                   !string.IsNullOrWhiteSpace(textBox3.Text) &&
                                   !string.IsNullOrWhiteSpace(textBox6.Text) &&
                                   !string.IsNullOrWhiteSpace(textBox4.Text);

            // Check that at least one radio button is selected
            bool radioSelected = radioButton1.Checked || radioButton2.Checked; // add more if you have more radio buttons

            if (textboxesFilled && radioSelected)
            {
                // All conditions met, process purchase
                ProcessPurchase();
            }
            else
            {
                MessageBox.Show("Please fill all required fields and select an option before proceeding with payment.",
                                "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Merchandise m = new Merchandise(UserId,Username);
            m.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Merchandise m = new Merchandise(UserId,Username);
            m.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Merchandise m = new Merchandise(UserId, Username);
            m.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Upcomings upcomings = new Upcomings(UserId, Username);
            upcomings.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Upcomings upcomings = new Upcomings(UserId, Username);
            upcomings.Show();
            this.Hide();
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            User_Home user_Home = new User_Home(UserId,Username);
            user_Home.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            User_Home user_Home = new User_Home(UserId, Username);
            user_Home.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BuyTicket buyTicket = new BuyTicket(UserId, Username);
            buyTicket.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            BuyTicket buyTicket = new BuyTicket(UserId, Username);
            buyTicket.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            User_Review user_Review = new User_Review(UserId, Username);
            user_Review.Show();
            this.Hide();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            User_Review user_Review = new User_Review(UserId, Username);
            user_Review.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            User_Booking_History UBH = new User_Booking_History(UserId, Username);
            UBH.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            User_Booking_History UBH = new User_Booking_History(UserId, Username);
            UBH.Show();
            this.Hide();
        }
    }
}
