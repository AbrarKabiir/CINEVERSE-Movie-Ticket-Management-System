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
using System.Windows.Forms.DataVisualization.Charting;

namespace Cineverse
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
            this.guna2GradientButton1.Click += new System.EventHandler(this.guna2GradientButton1_Click);
            this.guna2CircleButton1.Click += new System.EventHandler(this.guna2CircleButton1_Click);
            this.guna2GradientButton2.Click += new System.EventHandler(this.guna2GradientButton2_Click);
            this.guna2GradientButton3.Click += new System.EventHandler(this.guna2GradientButton3_Click); // Delete
            this.guna2GradientButton4.Click += new System.EventHandler(this.guna2GradientButton4_Click); // Edit
            this.guna2GradientButton5.Click += new System.EventHandler(this.guna2GradientButton5_Click); // Search
            this.Load += new System.EventHandler(this.AdminPanel_Load);
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox12.Image = Image.FromFile(ofd.FileName);
                pictureBox12.ImageLocation = ofd.FileName;
            }
        }

        private void pbPoster_Click(object sender, EventArgs e) { }
        private void btnUploadPoster_Click_1(object sender, EventArgs e) { }
        private void button2_Click(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void btnConfirm_Click(object sender, EventArgs e) { }
        private void button3_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
       
        
        private void AdminPanel_Load(object sender, EventArgs e)     /////////////////// LOAD EVENT ///////////////////
        {
            LoadUpcomingMoviesToGrid();
            LoadBookingHistory();
            ShowMostSoldMovie();
            LoadUsers();
            LoadReviews();
            LoadRevenueDashboard();
        }
        private void tabPage3_Click(object sender, EventArgs e) { }

       
        
        // ------------------------------------------------------   BOOKING HISTORY -----------------------------------------------------
       
        private void DTDRID1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        
        
        }
        
        string connString = "Data Source=ABRAR;Initial Catalog=CINE_VERSE_DB;Integrated Security=True;TrustServerCertificate=True";


        private DataTable bookingTable;

        private void LoadBookingHistory()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT MovieName, Theatre, ShowDate, ShowTime, SeatType, SeatNumbers, TotalPrice FROM Booking_History";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                bookingTable = new DataTable();
                da.Fill(bookingTable);

                DTDRID1.DataSource = bookingTable;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (bookingTable == null)
                return;

            string searchText = textBoxSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                // If empty, show all
                DTDRID1.DataSource = bookingTable;
                return;
            }

            // Use DataView to filter with case-insensitive search
            DataView dv = new DataView(bookingTable);
            dv.RowFilter = $"MovieName LIKE '%{searchText.Replace("'", "''")}%'"; // this works case-insensitive by default in SQL Server data

            DTDRID1.DataSource = dv;

        }

        private void ShowMostSoldMovie()
        {

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Count the number of seats for each movie
                string query = @"
            SELECT TOP 1 MovieName, SUM(LEN(SeatNumbers) - LEN(REPLACE(SeatNumbers, ',', '')) + 1) AS TicketsSold
            FROM Booking_History
            GROUP BY MovieName
            ORDER BY TicketsSold DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar(); // Get the first column of the first row

                    // If there is a result, display it
                    if (result != null)
                    {
                        // Since ExecuteScalar only returns the first column, get the movie name differently
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                label28.Text = reader["MovieName"].ToString();
                            }
                        }
                    }
                    else
                    {
                        label28.Text = "No bookings yet";
                    }
                }
            }
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------



        private void tabPage1_Click(object sender, EventArgs e) { }
        private void tabPage2_Click(object sender, EventArgs e) { }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }


        // ========================================================= NOW SHOWING MOVIES ============================================================

        private void BTNUPLOAD_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox6.Image = Image.FromFile(ofd.FileName); // show in AdminForm
                pictureBox6.Tag = ofd.FileName; // temporarily store path until update
            }
        }

        private void BTNMV1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NAME1.Text))
            {
                MessageBox.Show("Please enter a name!");
                return;
            }

            if (pictureBox6.Tag == null)
            {
                MessageBox.Show("Please upload a picture!");
                return;
            }

            string projectPath = Application.StartupPath;
            string imagesFolder = System.IO.Path.Combine(projectPath, "img");

            if (!System.IO.Directory.Exists(imagesFolder))
                System.IO.Directory.CreateDirectory(imagesFolder);

            string sourcePath = pictureBox6.Tag.ToString();
            string fileName = System.IO.Path.GetFileName(sourcePath);
            string destPath = System.IO.Path.Combine(imagesFolder, fileName);
            System.IO.File.Copy(sourcePath, destPath, true);

            string dataFile = System.IO.Path.Combine(projectPath, "AdminData.txt");
            List<string> lines = new List<string>();

            if (System.IO.File.Exists(dataFile))
                lines = System.IO.File.ReadAllLines(dataFile).ToList();

            while (lines.Count < 5)
                lines.Add("|");

            lines[0] = NAME1.Text + "|" + destPath;

            System.IO.File.WriteAllLines(dataFile, lines);

            MessageBox.Show("Movie 1 updated successfully!");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(ofd.FileName); // show in AdminForm
                pictureBox2.Tag = ofd.FileName; // temporarily store path until update
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox4.Image = Image.FromFile(ofd.FileName); // show in AdminForm
                pictureBox4.Tag = ofd.FileName; // temporarily store path until update
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.Image = Image.FromFile(ofd.FileName); // show in AdminForm
                pictureBox3.Tag = ofd.FileName; // temporarily store path until update
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox5.Image = Image.FromFile(ofd.FileName); // show in AdminForm
                pictureBox5.Tag = ofd.FileName; // temporarily store path until update
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please enter a name!");
                return;
            }

            if (pictureBox2.Tag == null)
            {
                MessageBox.Show("Please upload a picture!");
                return;
            }

            string projectPath = Application.StartupPath;
            string imagesFolder = System.IO.Path.Combine(projectPath, "img");

            if (!System.IO.Directory.Exists(imagesFolder))
                System.IO.Directory.CreateDirectory(imagesFolder);

            string sourcePath = pictureBox2.Tag.ToString();
            string fileName = System.IO.Path.GetFileName(sourcePath);
            string destPath = System.IO.Path.Combine(imagesFolder, fileName);
            System.IO.File.Copy(sourcePath, destPath, true);

            string dataFile = System.IO.Path.Combine(projectPath, "AdminData.txt");
            List<string> lines = new List<string>();

            if (System.IO.File.Exists(dataFile))
                lines = System.IO.File.ReadAllLines(dataFile).ToList();

            while (lines.Count < 5)
                lines.Add("|");

            lines[1] = textBox6.Text + "|" + destPath;

            System.IO.File.WriteAllLines(dataFile, lines);

            MessageBox.Show("Movie 2 updated successfully!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Please enter a name!");
                return;
            }

            if (pictureBox4.Tag == null)
            {
                MessageBox.Show("Please upload a picture!");
                return;
            }

            string projectPath = Application.StartupPath;
            string imagesFolder = System.IO.Path.Combine(projectPath, "img");

            if (!System.IO.Directory.Exists(imagesFolder))
                System.IO.Directory.CreateDirectory(imagesFolder);

            string sourcePath = pictureBox4.Tag.ToString();
            string fileName = System.IO.Path.GetFileName(sourcePath);
            string destPath = System.IO.Path.Combine(imagesFolder, fileName);
            System.IO.File.Copy(sourcePath, destPath, true);

            string dataFile = System.IO.Path.Combine(projectPath, "AdminData.txt");
            List<string> lines = new List<string>();

            if (System.IO.File.Exists(dataFile))
                lines = System.IO.File.ReadAllLines(dataFile).ToList();

            while (lines.Count < 5)
                lines.Add("|");

            lines[2] = textBox7.Text + "|" + destPath;

            System.IO.File.WriteAllLines(dataFile, lines);

            MessageBox.Show("Movie 3 updated successfully!");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox8.Text))
            {
                MessageBox.Show("Please enter a name!");
                return;
            }

            if (pictureBox3.Tag == null)
            {
                MessageBox.Show("Please upload a picture!");
                return;
            }

            string projectPath = Application.StartupPath;
            string imagesFolder = System.IO.Path.Combine(projectPath, "img");

            if (!System.IO.Directory.Exists(imagesFolder))
                System.IO.Directory.CreateDirectory(imagesFolder);

            string sourcePath = pictureBox3.Tag.ToString();
            string fileName = System.IO.Path.GetFileName(sourcePath);
            string destPath = System.IO.Path.Combine(imagesFolder, fileName);
            System.IO.File.Copy(sourcePath, destPath, true);

            string dataFile = System.IO.Path.Combine(projectPath, "AdminData.txt");
            List<string> lines = new List<string>();

            if (System.IO.File.Exists(dataFile))
                lines = System.IO.File.ReadAllLines(dataFile).ToList();

            while (lines.Count < 5)
                lines.Add("|");

            lines[3] = textBox8.Text + "|" + destPath;

            System.IO.File.WriteAllLines(dataFile, lines);

            MessageBox.Show("Movie 4 updated successfully!");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox9.Text))
            {
                MessageBox.Show("Please enter a name!");
                return;
            }

            if (pictureBox5.Tag == null)
            {
                MessageBox.Show("Please upload a picture!");
                return;
            }

            string projectPath = Application.StartupPath;
            string imagesFolder = System.IO.Path.Combine(projectPath, "img");

            if (!System.IO.Directory.Exists(imagesFolder))
                System.IO.Directory.CreateDirectory(imagesFolder);

            string sourcePath = pictureBox5.Tag.ToString();
            string fileName = System.IO.Path.GetFileName(sourcePath);
            string destPath = System.IO.Path.Combine(imagesFolder, fileName);
            System.IO.File.Copy(sourcePath, destPath, true);

            string dataFile = System.IO.Path.Combine(projectPath, "AdminData.txt");
            List<string> lines = new List<string>();

            if (System.IO.File.Exists(dataFile))
                lines = System.IO.File.ReadAllLines(dataFile).ToList();

            while (lines.Count < 5)
                lines.Add("|");

            lines[4] = textBox9.Text + "|" + destPath;

            System.IO.File.WriteAllLines(dataFile, lines);

            MessageBox.Show("Movie 5 updated successfully!");
        }

        // =======================================================================================================================================



        // ========================================================= UPCOMING MOVIES ============================================================

        private void Upcoming_Movies_Click(object sender, EventArgs e) { }

        private void guna2GradientButton2_Click(object sender, EventArgs e) // ADD
        {
            if (!AreUpcomingFieldsFilled())
            {
                MessageBox.Show("Please fill all fields and upload poster/trailer.");
                return;
            }
            string name = guna2TextBox1.Text;
            string genre = guna2ComboBox1.SelectedItem.ToString();
            int year = int.Parse(guna2TextBox2.Text);
            double rating = double.Parse(guna2TextBox3.Text);
            string posterFile = pictureBox12.ImageLocation;
            string trailerUrl = guna2TextBox4.Text;

            string connectionString = @"Server=ABRAR;Database=CINE_VERSE_DB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Upcoming_Movies (MovieName, Genre, Year, Rating, PosterFile, TrailerUrl)
                                 VALUES (@name, @genre, @year, @rating, @posterFile, @trailerUrl)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@genre", genre);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@rating", rating);
                    cmd.Parameters.AddWithValue("@posterFile", posterFile);
                    cmd.Parameters.AddWithValue("@trailerUrl", trailerUrl);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Upcoming movie added successfully!");
            LoadUpcomingMoviesToGrid();
            ClearUpcomingFields();
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Video Files|*.mp4;*.avi;*.mov;*.wmv|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                guna2TextBox4.Text = ofd.FileName;
            }
        }

        private bool AreUpcomingFieldsFilled()
        {
            return !string.IsNullOrWhiteSpace(guna2TextBox1.Text) && // Movie Name
                   guna2ComboBox1.SelectedItem != null &&             // Genre
                   !string.IsNullOrWhiteSpace(guna2TextBox2.Text) &&  // Year
                   !string.IsNullOrWhiteSpace(guna2TextBox3.Text) &&  // Rating
                   !string.IsNullOrWhiteSpace(guna2TextBox4.Text) &&  // Trailer Path
                   !string.IsNullOrWhiteSpace(pictureBox12.ImageLocation); // Poster Path
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e) // EDIT
        {
            string name = guna2TextBox1.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter the movie name to update.");
                return;
            }

            List<string> setClauses = new List<string>();
            var parameters = new List<SqlParameter>();

            if (guna2ComboBox1.SelectedItem != null)
            {
                setClauses.Add("Genre=@genre");
                parameters.Add(new SqlParameter("@genre", guna2ComboBox1.SelectedItem.ToString()));
            }
            if (!string.IsNullOrWhiteSpace(guna2TextBox2.Text))
            {
                setClauses.Add("Year=@year");
                parameters.Add(new SqlParameter("@year", int.Parse(guna2TextBox2.Text)));
            }
            if (!string.IsNullOrWhiteSpace(guna2TextBox3.Text))
            {
                setClauses.Add("Rating=@rating");
                parameters.Add(new SqlParameter("@rating", double.Parse(guna2TextBox3.Text)));
            }
            if (!string.IsNullOrWhiteSpace(pictureBox12.ImageLocation))
            {
                setClauses.Add("PosterFile=@posterFile");
                parameters.Add(new SqlParameter("@posterFile", pictureBox12.ImageLocation));
            }
            if (!string.IsNullOrWhiteSpace(guna2TextBox4.Text))
            {
                setClauses.Add("TrailerUrl=@trailerUrl");
                parameters.Add(new SqlParameter("@trailerUrl", guna2TextBox4.Text));
            }

            if (setClauses.Count == 0)
            {
                MessageBox.Show("Please fill at least one field to update.");
                return;
            }

            string setClause = string.Join(", ", setClauses);
            string query = $"UPDATE Upcoming_Movies SET {setClause} WHERE MovieName=@name";
            parameters.Add(new SqlParameter("@name", name));

            string connectionString = @"Server=ABRAR;Database=CINE_VERSE_DB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Movie updated successfully!");
            LoadUpcomingMoviesToGrid();
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e) // DEL
        {
            string name = guna2TextBox1.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Enter the movie name to delete.");
                return;
            }
            string connectionString = @"Server=ABRAR;Database=CINE_VERSE_DB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"DELETE FROM Upcoming_Movies WHERE MovieName=@name";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Movie deleted successfully!");
            LoadUpcomingMoviesToGrid();
        }

        private void guna2GradientButton5_Click(object sender, EventArgs e) // SEARCH
        {
            string search = guna2TextBox1.Text.Trim();
            string connectionString = @"Server=ABRAR;Database=CINE_VERSE_DB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Upcoming_Movies WHERE MovieName LIKE @search";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }
        }

        private void LoadUpcomingMoviesToGrid()
        {
            string connectionString = @"Server=ABRAR;Database=CINE_VERSE_DB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Upcoming_Movies";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView2.DataSource = dt;
                }
            }
        }

        private void ClearUpcomingFields()
        {
            guna2TextBox1.Text = "";
            guna2ComboBox1.SelectedIndex = -1;
            guna2TextBox2.Text = "";
            guna2TextBox3.Text = "";
            guna2TextBox4.Text = "";
            pictureBox12.Image = null;
            pictureBox12.ImageLocation = "";
        }

        // =====================================================================================================================================

        
        private void button16_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button15_Click(object sender, EventArgs e)
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

        private void button17_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // ========================================================== USER INFORMATION ============================================================


        private void guna2GradientButton1_Click_1(object sender, EventArgs e)
        {

        }

        private DataTable usersTable;

        private void LoadUsers()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserID, Username,Email,DateOfBirth FROM User_Details";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                usersTable = new DataTable();
                da.Fill(usersTable);
                dataGridView1.DataSource = usersTable;
            }
        }

        private void guna2GradientButton8_Click(object sender, EventArgs e)
        {
            string userIdText = guna2TextBox5.Text.Trim();
            string usernameText = guna2TextBox6.Text.Trim();

            if (string.IsNullOrWhiteSpace(userIdText) && string.IsNullOrWhiteSpace(usernameText))
            {
                MessageBox.Show("Enter a UserID or Username to search.");
                return;
            }

            DataView dv = new DataView(usersTable);

            if (!string.IsNullOrWhiteSpace(userIdText))
            {
                dv.RowFilter = $"Convert(UserID, 'System.String') = '{userIdText.Replace("'", "''")}'";
            }
            else if (!string.IsNullOrWhiteSpace(usernameText))
            {
                dv.RowFilter = $"Username LIKE '%{usernameText.Replace("'", "''")}%'"; // partial match
            }

            dataGridView1.DataSource = dv;
        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox5.Text))
            {
                MessageBox.Show("Enter UserID to update.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"UPDATE USER_DETAILS 
                         SET Username = ISNULL(NULLIF(@Username,''), Username),
                             Email = ISNULL(NULLIF(@Email,''), Email),
                             DateOfBirth = @DateOfBirth
                         WHERE UserID = @UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", guna2TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@Username", string.IsNullOrWhiteSpace(guna2TextBox6.Text) ? "" : guna2TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(guna2TextBox7.Text) ? "" : guna2TextBox7.Text.Trim());
                cmd.Parameters.AddWithValue("@DateOfBirth", guna2DateTimePicker1.Value);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("User updated successfully!");
                else
                    MessageBox.Show("UserID not found.");

                LoadUsers();
            }
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(guna2TextBox5.Text))
            {
                MessageBox.Show("Enter UserID to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this user?",
                                                  "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "DELETE FROM User_Details WHERE UserID=@UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", guna2TextBox5.Text.Trim());

                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                        MessageBox.Show("User deleted successfully!");
                    else
                        MessageBox.Show("UserID not found.");

                    LoadUsers();
                }
            }
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        // =====================================================================================================================================


        // ========================================================== REVIEW ============================================================================


        private void LoadReviews()
        {
            flowLayoutPanel1.Controls.Clear();

            string connectionString = "Data Source=ABRAR;Initial Catalog=CINE_VERSE_DB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            string sql = @"SELECT ReviewId, UserId, Username, MovieName, ShopName, Rating, Comment, ReviewDate
                   FROM Reviews
                   ORDER BY ReviewDate DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var card = new ReviewCard();

                        string username = dr["Username"] == DBNull.Value ? "Unknown" : dr["Username"].ToString();

                        int rating = 0;
                        if (dr["Rating"] != DBNull.Value)
                        {
                            rating = dr["Rating"] != DBNull.Value ? Convert.ToInt32(dr["Rating"]) : 0;
                        }

                        string comment = dr["Comment"] == DBNull.Value ? "" : dr["Comment"].ToString();
                        DateTime reviewDate = dr["ReviewDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(dr["ReviewDate"]);
                        string movieName = dr["MovieName"] == DBNull.Value ? null : dr["MovieName"].ToString();
                        string shopName = dr["ShopName"] == DBNull.Value ? null : dr["ShopName"].ToString();

                        card.SetReview(username, rating, reviewDate, movieName, shopName, comment);

                        // Stretch to FlowLayoutPanel width
                        card.Width = flowLayoutPanel1.ClientSize.Width - 20;
                        card.Margin = new Padding(6);
                        card.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                        flowLayoutPanel1.Controls.Add(card);
                    }
                }
            }
        }

        // =====================================================================================================================================


        // ======================================================= COMMISION ============================================================


        private string connectionString = @"Data Source=ABRAR;Initial Catalog=CINE_VERSE_DB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";

        private (decimal tickets, decimal items) GetTotals()
        {
            decimal tickets = 0m;
            decimal items = 0m;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(TotalPrice),0) * 0.25 FROM Booking_History", conn))
                {
                    tickets = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                using (SqlCommand cmd = new SqlCommand("SELECT ISNULL(SUM(TotalPrice),0) * 0.25 FROM SalesHistory", conn))
                {
                    items = Convert.ToDecimal(cmd.ExecuteScalar());
                }
            }

            return (tickets, items);
        }

        private decimal GetTotalRevenueDays(int daysBack, int skipDays)
        {
            decimal total = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = $@"SELECT (SELECT ISNULL(SUM(TotalPrice),0) * 0.25 FROM Booking_History
                             WHERE BookingDate BETWEEN DATEADD(DAY, -{daysBack}, GETDATE()) AND DATEADD(DAY, -{skipDays}, GETDATE()))
                             +
                             (SELECT ISNULL(SUM(TotalPrice),0) * 0.25 FROM SalesHistory
                             WHERE SaleDate BETWEEN DATEADD(DAY, -{daysBack}, GETDATE()) AND DATEADD(DAY, -{skipDays}, GETDATE())) ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                    total = Convert.ToDecimal(cmd.ExecuteScalar());
            }

            return total;
        }
        private DataTable GetDailyRevenue(int days = 7)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = $@"SELECT CAST([Date] AS DATE) AS [Date], SUM(TotalPrice * 0.25) AS Revenue FROM (
                SELECT BookingDate AS [Date], TotalPrice FROM Booking_History
                WHERE BookingDate >= DATEADD(DAY, -{days}, GETDATE())
                UNION ALL
                SELECT SaleDate AS [Date], TotalPrice FROM SalesHistory
                WHERE SaleDate >= DATEADD(DAY, -{days}, GETDATE()) ) t
                GROUP BY CAST([Date] AS DATE)
                ORDER BY CAST([Date] AS DATE)  ";

                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }




        private void ConfigureCharts()
        {
            // Pie chart
            if (chart1.ChartAreas.Count == 0) chart1.ChartAreas.Add("CA");
            if (chart1.Legends.Count == 0) chart1.Legends.Add(new Legend("L"));
            chart1.Legends[0].Docking = Docking.Right;

            // Column chart
            if (chart2.ChartAreas.Count == 0) chart2.ChartAreas.Add("CA2");
            if (chart2.Legends.Count == 0) chart2.Legends.Add(new Legend("L2"));
            chart2.Legends[0].Enabled = false;
            chart2.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chart2.ChartAreas[0].AxisX.Interval = 1;
        }

        private void LoadRevenueDashboard()
        {
            try
            {
                // 1) totals
                var (ticketsRevenue, itemsRevenue) = GetTotals();
                decimal total = ticketsRevenue + itemsRevenue;

                // 2) update big label and KPI labels
                label31.Text = $"Total Revenue: {total:N0}";
                decimal pctTickets = total == 0 ? 0m : Math.Round(ticketsRevenue * 100m / total, 1);
                decimal pctItems = total == 0 ? 0m : Math.Round(itemsRevenue * 100m / total, 1);

                label32.Text = $"Tickets: {ticketsRevenue:N0} ({pctTickets:N1}%)";
                label33.Text = $"Items:   {itemsRevenue:N0} ({pctItems:N1}%)";

                // 3) update circle progress bars (Guna2CircleProgressBar expects int between 0-100)
                int ipctTickets = (int)Math.Round(pctTickets);
                int ipctItems = (int)Math.Round(pctItems);
                ipctTickets = Math.Max(0, Math.Min(100, ipctTickets));
                ipctItems = Math.Max(0, Math.Min(100, ipctItems));

                // 4) populate Pie Chart
                chart1.Series.Clear();
                var sPie = chart1.Series.Add("Share");
                sPie.ChartType = SeriesChartType.Pie;
                sPie["PieLabelStyle"] = "Outside";                // labels outside
                sPie.IsValueShownAsLabel = true;

                var p1 = sPie.Points.AddXY("Tickets", Convert.ToDouble(ticketsRevenue));
                var p2 = sPie.Points.AddXY("Items", Convert.ToDouble(itemsRevenue));


                // 5) populate Column chart with last 7 days
                DataTable dt = GetDailyRevenue(7); // last 7 days
                chart2.Series.Clear();
                var sCol = chart2.Series.Add("DailyRevenue");
                sCol.ChartType = SeriesChartType.Column;
                sCol.IsValueShownAsLabel = true;
                sCol["PointWidth"] = "0.6";

                // Ensure we plot in ascending date order
                foreach (DataRow row in dt.Rows)
                {
                    DateTime date = Convert.ToDateTime(row["Date"]);
                    double revenue = Convert.ToDouble(row["Revenue"]);
                    string label = date.ToString("dd MMM"); // e.g. 05 Sep
                    var pt = sCol.Points.AddXY(label, revenue);

                }

                chart1.Invalidate();
                chart2.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load revenue: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            decimal lastWeek = GetTotalRevenueDays(14, 7);
            decimal thisWeek = GetTotalRevenueDays(7, 0);
            decimal growth = lastWeek == 0 ? 100 : ((thisWeek - lastWeek) * 100m / lastWeek);
            label34.Text = $"Growth: {growth:N1}%";

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        // ================================================================================================================================








    }
}
