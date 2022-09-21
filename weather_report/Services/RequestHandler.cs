using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace weather_report.Services
{
    public static class RequestHandler
    {
        public static async Task<string> SendGetRequestAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (String.IsNullOrEmpty(content))
                    {
                        throw new InvalidOperationException("Error: empty response from server");
                    }
                    else
                    {
                        return content;
                    }
                }
                else
                {
                    throw new HttpRequestException("Server error response");
                }
            }
        }
    }
}
