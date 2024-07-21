using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace TopCrypto.DataLayer.Services.Interfaces
{
    public abstract class AbstractEndPointService
    {
        public async Task<HttpResponseMessage> GetResponse(string url, Dictionary<string, string> queryParams = null,
            Dictionary<string, string> headers = null)
        {
            var uriBuilder = new UriBuilder(url);

            if (queryParams != null && queryParams.Count > 0)
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                foreach (var pair in queryParams)
                {
                    query[pair.Key] = pair.Value;
                }
                uriBuilder.Query = query.ToString();
            }

            using (var client = new HttpClient())
            {
                var defaulHeadersCollection = client.DefaultRequestHeaders;
                if (headers != null && headers.Count > 0)
                {
                    foreach (var pair in headers)
                    {
                        defaulHeadersCollection.Add(pair.Key, pair.Value);
                    }
                }

                return await client.GetAsync(uriBuilder.ToString());
            }
        }
    }
}
