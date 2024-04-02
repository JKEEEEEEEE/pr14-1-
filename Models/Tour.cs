using System;
using System.Collections.Generic;

namespace diplom_api.Models
{
    public partial class Tour
    {
        public int IdTours { get; set; }
        public string NameTours { get; set; } = null!;
        public string Photo { get; set; } = null!;
        public string DescriptionTours { get; set; } = null!;
        public string Place { get; set; } = null!;
        public DateTime StarteDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string TourType { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string NumberAvailable { get; set; } = null!;
        public int? ReviewsId { get; set; }
        public int? TouristRoutesId { get; set; }

    }
}
