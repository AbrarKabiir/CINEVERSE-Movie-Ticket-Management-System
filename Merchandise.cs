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

namespace Cineverse
{
    public partial class Merchandise : Form
    {
        private int UserId; 
        private string Username;

        public Merchandise(int userId, string username)
        {
            InitializeComponent();
            UserId = userId;

            this.Load += new System.EventHandler(this.Merchandise_Load);
            Username = username;
        }
        private void Merchandise_Load(object sender, EventArgs e)
        {
            SetupGrid();
            LoadAllItems();
        }

        private void SetupGrid()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            dataGridView1.RowTemplate.Height = 100;

            dataGridView1.Columns.Add("ItemID", "ItemID"); 
            dataGridView1.Columns["ItemID"].Visible = false;

            dataGridView1.Columns.Add("ItemName", "Item Name");
            dataGridView1.Columns.Add("Price", "Price");
            dataGridView1.Columns.Add("AvailableQty", "Stock");
            dataGridView1.Columns.Add("ShopName", "Shop Name");

            dataGridView1.Columns.Add("VendorID", "VendorID"); // Hidden
            dataGridView1.Columns["VendorID"].Visible = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 11);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);




            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol.Name = "ItemImage";
            imgCol.HeaderText = "Image";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom; // keeps image proportional
            dataGridView1.Columns.Add(imgCol);

            // Checkbox to select item
            DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
            chkCol.Name = "SelectItem";
            chkCol.HeaderText = "Select";
            dataGridView1.Columns.Add(chkCol);

            // Numeric column for quantity
            DataGridViewTextBoxColumn qtyCol = new DataGridViewTextBoxColumn();
            qtyCol.Name = "BuyQty";
            qtyCol.HeaderText = "Quantity";
            dataGridView1.Columns.Add(qtyCol);
        }

        public void LoadAllItems()
        {
            string connString = "Data Source=ABRAR;Initial Catalog=CINE_VERSE_DB;Integrated Security=True;TrustServerCertificate=True";
            
            string query = @"SELECT i.ItemID, i.ItemName, i.Price, i.Quantity,v.ShopName, i.VendorID, i.Picture
                            FROM Items i
                            INNER JOIN Vendor v ON i.VendorID = v.VendorID";
        
        
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    Image img = null;
                    string imgPath = row["Picture"].ToString();
                    if (System.IO.File.Exists(imgPath))
                    {
                        img = Image.FromFile(imgPath);
                    }

                    dataGridView1.Rows.Add(
                        row["ItemID"],
                        row["ItemName"],
                        row["Price"] + " Tk",
                        row["Quantity"],   // <-- from Items
                        row["ShopName"],   // <-- now works, comes from Vendor
                        row["VendorID"],
                        img,
                        false,
                        0
                    );
                }
            }
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private int CalculateTotalPrice()
        {
            int total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool isChecked = row.Cells["SelectItem"].Value != null && (bool)row.Cells["SelectItem"].Value;

                if (isChecked && row.Cells["BuyQty"].Value != null)
                {
                    int buyQty = Convert.ToInt32(row.Cells["BuyQty"].Value);
                    int price = Convert.ToInt32(row.Cells["Price"].Value.ToString().Replace(" Tk", ""));

                    if (buyQty > 0)
                    {
                        total += price * buyQty;
                    }
                }
            }

            return total;
        }


        private void button9_Click(object sender, EventArgs e)
        {
            int totalPrice = CalculateTotalPrice();

            if (totalPrice > 0)
            {
                Merchandise_Pay mp = new Merchandise_Pay(totalPrice,UserId,username);
                mp.Owner = this;
                mp.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select items and enter quantity.");
            }

        }

        private void Merchandise_Load_1(object sender, EventArgs e)
        {

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

        private string username;
        private void button2_Click(object sender, EventArgs e)
        {
            User_Home home = new User_Home(UserId,username);
            home.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            User_Home home = new User_Home(UserId, username);
            home.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BuyTicket bt = new BuyTicket(UserId, username);
            bt.Show();
            this.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            BuyTicket bt = new BuyTicket(UserId, username);
            bt.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Upcomings up = new Upcomings(UserId, Username); 
            up.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Upcomings up = new Upcomings(UserId, Username);
            up.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            User_Review user_Review = new User_Review(UserId, username);
            user_Review.Show();
            this.Hide();
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            User_Review user_Review = new User_Review(UserId, username);
            user_Review.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {

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

