using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayPal.Shared
{
    public class ResponseList<T> where T : class
    {
        [JsonProperty("total_items")]
        public int TotalItems { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        public IReadOnlyCollection<T> Data { get; set; }
    }
}