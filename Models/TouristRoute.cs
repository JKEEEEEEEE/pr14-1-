using System;
using System.Collections.Generic;

namespace diplom_api.Models
{
    public partial class TouristRoute
    {

        public int IdTouristRoutes { get; set; }
        public string RouteDescription { get; set; } = null!;
        public DateTime TokenDateTime { get; set; }
        public string PlacesVisited { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public string Attractions { get; set; } = null!;

    }
}
