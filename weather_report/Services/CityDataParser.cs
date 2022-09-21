using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weather_report.Interfaces;

namespace weather_report.Services
{
    public class CityDataParser<T> : IStringDataParser<T> where T : class
    {
        public List<T>? Data { get; set; }
        public List<T> GetDataListFromString(string content)
        {
            Data = JsonConvert.DeserializeObject<List<T>>(content);
            return Data;
        }
    }
}
