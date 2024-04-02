using System;
using System.Collections.Generic;

namespace diplom_api.Models
{
    public partial class Token
    {
        public int IdToken { get; set; }
        public string Token1 { get; set; } = null!;
        public DateTime TokenDateTime { get; set; }
    }
}