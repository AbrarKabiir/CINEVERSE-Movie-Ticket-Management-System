using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineverse
{
    internal class BookingInfo
    {
   
            public string MovieName { get; set; }
            public string TheatreName { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public List<string> Seats { get; set; }
            public string SeatType { get; set; }
            public decimal Price { get; set; }
            public int UserId { get; set; } // optional if you track login
        

    }
}
