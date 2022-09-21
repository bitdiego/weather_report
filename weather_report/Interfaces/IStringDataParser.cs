using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weather_report.Interfaces
{
    public interface IStringDataParser<T> where T : new()
    {
        List<T> GetDataListFromString(string content);
    }
}
