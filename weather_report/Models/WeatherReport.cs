using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weather_report.Models
{
    public class WeatherReport
    {
        public double Maxtemp_c { get; set; }
        public double Maxtemp_f { get; set; }
        public double Mintemp_c { get; set; }
        public double Mintemp_f { get; set; }
        public double Avgtemp_c { get; set; }
        public double Avgtemp_f { get; set; }
        public double Maxwind_mph { get; set; }
        public double Maxwind_kph { get; set; }
        public double Totalprecip_mm { get; set; }
        public double Totalprecip_in { get; set; }
        public double Avgvis_km { get; set; }
        public double Avgvis_miles { get; set; }
        public double Avghumidity { get; set; }
        public double Daily_will_it_rain { get; set; }
        public double Daily_chance_of_rain { get; set; }
        public double Daily_will_it_snow { get; set; }
        public double Daily_chance_of_snow { get; set; }
        public double Uv { get; set; }
        public string Text { get; set; } = "";
        public string Icon { get; set; } = "";
        public int Code { get; set; }
    }
}
