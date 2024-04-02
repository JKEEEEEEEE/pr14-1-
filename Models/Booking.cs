using System;
using System.Collections.Generic;

namespace diplom_api.Models
{
    public partial class Booking
    {
        public int IdBooking { get; set; }
        public string ReservationNumber { get; set; } = null!;
        public DateTime BookingDate { get; set; }
        public string BookingStatus { get; set; } = null!;
        public string NumberOfBooked { get; set; } = null!;
        public DateTime StarteDate { get; set; }
        public int? ToursId { get; set; }
        public int? UsersId { get; set; }


    }
}
