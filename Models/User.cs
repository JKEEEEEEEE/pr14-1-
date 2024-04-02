using System;
using System.Collections.Generic;

namespace diplom_api.Models
{
    public partial class User
    {
        public int IdUsers { get; set; }
        public string FirstNameUsers { get; set; } = null!;
        public string SecondNameUsers { get; set; } = null!;
        public string? MiddleNameUsers { get; set; }
        public string MailUsers { get; set; } = null!;
        public string LoginUsers { get; set; } = null!;
        public string PasswordUsers { get; set; } = null!;
        public string? Salt { get; set; }
        public int? RoleId { get; set; }

    }
}