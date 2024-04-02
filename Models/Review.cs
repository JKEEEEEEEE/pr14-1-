using System;
using System.Collections.Generic;

namespace diplom_api.Models
{
    public partial class Review
    {

        public int IdReviews { get; set; }
        public string Grade { get; set; } = null!;
        public string ReviewDescription { get; set; } = null!;
        public DateTime DateReviews { get; set; }
        public string MailUsers { get; set; } = null!;
        public int? UsersId { get; set; }


    }
}