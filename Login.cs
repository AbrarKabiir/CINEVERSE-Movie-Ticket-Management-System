using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cineverse
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Signup sp = new Signup();
            sp.Show();
            this.Hide();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Home hp = new Home();
            hp.Show();
            this.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';

            }
            else
            {
                textBox2.PasswordChar = '●';
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Home hm = new Home();
            hm.Show();
            this.Hide();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter Username and Password");
                return;
            }

            string ConnString = "Data Source=ABRAR;Initial Catalog=CINE_VERSE_DB;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";


            SqlConnection conn = new SqlConnection(ConnString);
            conn.Open();

            string hashedPassword = HashPassword(textBox2.Text);

            SqlCommand cmd = new SqlCommand("SELECT UserId,Role,Username FROM User_Details WHERE Username=@Username COLLATE SQL_Latin1_General_CP1_CS_AS AND Password=@Password", conn);
            cmd.Parameters.AddWithValue("@Username", textBox1.Text);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string role = dt.Rows[0]["Role"].ToString().Trim().ToUpper();
                MessageBox.Show("Login Successful! Role: " + role);

                if (role == "USER")
                {
                    int userId = Convert.ToInt32(dt.Rows[0]["UserId"]);
                    string username = dt.Rows[0]["Username"].ToString();
                    User_Home uh = new User_Home(userId, username);
                    uh.Show();
                    this.Hide();
                }
                else if (role == "VENDOR")
                {
                    int userID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                    VEndor2 v = new VEndor2(userID);
                    v.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Invalid Username or Password");
            }

            conn.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_Click(object sender, EventArgs e)
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
    }
}
