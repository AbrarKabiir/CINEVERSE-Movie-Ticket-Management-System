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
    public partial class ReviewCard : UserControl
    {
        public ReviewCard()
        {
            InitializeComponent();
        }

        private void ReviewCard_Load(object sender, EventArgs e)
        {

        }

        public void SetReview(string username, int rating, DateTime date, string movieName, string shopName, string comment)
        {
            label1.Text = string.IsNullOrWhiteSpace(username) ? "Unknown" : username;

            BuildStars(rating);

            string title = !string.IsNullOrWhiteSpace(movieName) ? "Movie: " + movieName
                         : !string.IsNullOrWhiteSpace(shopName) ? "Shop: " + shopName
                         : "Unknown";

            label4.Text = title;              // Movie or Shop title
            label3.Text = date.ToString("dd-MMM-yyyy");

            label2.Text = comment ?? string.Empty;

        }

        private void BuildStars(int rating)
        {
            panel2.Controls.Clear();
            rating = Math.Max(0, Math.Min(5, rating)); // clamp 0..5

            for (int i = 1; i <= 5; i++)
            {
                PictureBox star = new PictureBox
                {
                    Size = new Size(16, 16),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Margin = new Padding(0, 0, 4, 0),
                    Image = (i <= rating) ? Properties.Resources.star_10134048 : Properties.Resources.image__2_

                   
                };
                star.Location = new Point((i - 1) * 20, 0); // 20 = width + spacing
                panel2.Controls.Add(star);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
