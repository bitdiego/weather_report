using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weather_report.Models
{
    internal class City
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public bool Top { get; set; }
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public string Content { get; set; } = "";
        public string Meta_description { get; set; } = "";
        public string Meta_title { get; set; } = "";
        public string Headline { get; set; } = "";
        public double Weight { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Cover_image_url { get; set; } = "";
        public string Url { get; set; } = "";
        public int Activities_count { get; set; }
        public string Time_zone { get; set; } = "";
        public int List_count { get; set; }
        public int Venue_count { get; set; }
        public bool Show_in_popular { get; set; } = false;
        public Country? Country { get; set; }
    }
}