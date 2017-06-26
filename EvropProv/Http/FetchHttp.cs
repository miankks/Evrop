using EvropProv.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace EvropProv.Http
{
    public class FetchHttp : IHttpFetcher
    {
        public IEnumerable<Data> GetData(string baseUrl, string apiUrl, string name, string date)
        {
            var objectList = new List<Data>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    //Get http response from apiurl
                    var response = client.GetAsync(apiUrl).Result;
                    response.EnsureSuccessStatusCode();

                    //To read json
                    var content = response.Content.ReadAsStringAsync().Result;

                    PopulateToList(objectList, content);
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.ToString());
            }

            var splitDate = date.Split('-');
            var filteredResult = (from selected in objectList
                                  where selected.Customer.Contains(name) &&
                                  selected.Date.Year == Convert.ToInt32(splitDate[0]) &&
                                  selected.Date.Month == Convert.ToInt32(splitDate[1])
                                  select selected).ToList();

            return filteredResult;
        }

        private void PopulateToList(ICollection<Data> objectList, string content)
        {

            var reader = new JsonTextReader(new StringReader(content))
            {
                SupportMultipleContent = true,
                Culture = CultureInfo.InvariantCulture,
                FloatParseHandling = FloatParseHandling.Double
            };

            while (reader.Read())
            {
                var serializer = new JsonSerializer { Culture = CultureInfo.InvariantCulture };
                var model = serializer.Deserialize<Data>(reader);
                objectList.Add(model);
            }
            reader.Close();
        }
        public static double GetBalance(IEnumerable<Data> dataObj)
        {
            var transactions = from transaction in dataObj
                               select transaction.Transaction;

            return transactions.Sum();
        }
    }
}