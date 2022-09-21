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
    public class DynamicDataParser<T> : IStringDataParser<T> where T : new()
    {
        public List<T>? Data { get; set; }
        public List<T> GetDataListFromString(string content)
        {
            Data = new List<T>();
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(content);
            //cityName = Convert.ToString(obj["location"]["name"]);
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
            var _keyName = key.Substring(0, 1).ToUpper() + key.Substring(1);
            PropertyInfo prop = wp.GetType().GetProperty(_keyName);
            var propertyName = prop.PropertyType.Name;
            if (propertyName == "Double")
            {
                prop.SetValue(wp, Convert.ToDouble(value), null);
            }
            else if (propertyName == "String")
            {
                prop.SetValue(wp, Convert.ToString(value), null);
            }
            else if (propertyName == "Int32")
            {
                prop.SetValue(wp, Convert.ToInt32(value), null);
            }
        }
    }
}
