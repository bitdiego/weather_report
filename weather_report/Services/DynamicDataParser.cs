using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using weather_report.Interfaces;

namespace weather_report.Services
{
    public class DynamicDataParser<T> : IStringDataParser<T> where T : class, new()
    {
        public List<T>? Data { get; set; }
        public List<T> GetDataListFromString(string content)
        {
            Data = new List<T>();
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(content);
            var forecast = obj["forecast"];
            var days = obj["forecast"]["forecastday"];

            foreach (var day in days)
            {
                //Console.WriteLine(day["date"]);
                //Console.WriteLine(x["day"]+" "+ x["day"].GetType());
                IList<string> keys = ((JObject)day["day"]).Properties().Select(par => par.Name).ToList();
                T wp = new T();
                foreach (string key in keys)
                {
                    if (day["day"][key] is JValue)
                    {
                        SetPropertyValue(key, day["day"][key], wp);
                    }
                    else if (day["day"][key] is JObject)
                    {
                        IList<string> inkeys = ((JObject)day["day"][key]).Properties().Select(par => par.Name).ToList();
                        foreach (string kk in inkeys)
                        {
                            SetPropertyValue(kk, day["day"][key][kk], wp);
                        }
                    }
                }
                Data.Add(wp);
            }

            return Data;
        }

        private static void SetPropertyValue(string key, JValue value, T wp)
        {
            var _keyName = key.Capitalize();
            PropertyInfo prop = wp.GetType().GetProperty(_keyName);

            prop.SetValue(wp, Convert.ChangeType(value, prop.PropertyType));
        }
    }
}
