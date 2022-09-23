using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weather_report
{
    public static class Globals
    {
        public static readonly string TUI_CITIES_URL = "https://api.musement.com/api/v3/cities/";
        public static readonly string WEATHER_FORECAST_API_URL = "https://api.weatherapi.com/v1/forecast.json?key={0}&q={1}&days=2&aqi=no&alerts=no";
        public static string WEATHER_API_KEY { get; set; } = "7ad1890196274ff89ea154035221209";

        public static string Capitalize(this string str)
        {
            var newString = str.Substring(0, 1).ToUpper() + str.Substring(1);
            return newString;
        }
    }
}
