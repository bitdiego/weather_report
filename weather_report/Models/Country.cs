using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weather_report.Models
{
    internal class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Iso_code { get; set; } = "";
    }
}
