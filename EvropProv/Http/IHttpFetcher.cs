using EvropProv.Model;
using System.Collections.Generic;

namespace EvropProv.Http
{
    public interface IHttpFetcher
    {
        IEnumerable<Data> GetData(string baseUrl, string apiUrl, string name, string date);
    }
}
