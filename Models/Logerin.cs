using System;
using System.Collections.Generic;

namespace diplom_api.Models
{
    public partial class Logerin
	{
        public int IdLogs { get; set; }
        public string Levels { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Exception { get; set; } = null!;
        public DateTime TimeDate { get; set; }
    }
}
