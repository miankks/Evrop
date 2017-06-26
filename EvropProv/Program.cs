using EvropProv.Http;
using System;
using EvropProv.Model;
namespace EvropProv
{
    public class Program
    {
        public static void Main()
        {
           
            var customer = new Data();
            var fetcher = new FetchHttp();
            const string baseUrl = "https://www.poromaa.com/assignment_data/";
            const string apiUrl = "transactions.json";
            string[] transAndDate = { "", "2016-09-25T15:12:20.5251805+02:00" };

            Console.WriteLine("Please wait, Loading data...");
            //Get list with objects
            var data = fetcher.GetData(baseUrl, apiUrl, transAndDate[0], transAndDate[1]);
            var balance = FetchHttp.GetBalance(data);
            Console.Clear();
            Console.WriteLine($"Balance: {balance}");

            Console.ReadKey();
          
        }
    }
}