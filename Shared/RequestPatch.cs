using Newtonsoft.Json;

namespace PayPal.Shared
{
    public class RequestPatch
    {
        public RequestPatch()
        {
            Operation = "replace";
        }

        [JsonProperty("op")]
        public string Operation { get; private set; }

        [JsonProperty("path")]
        public string Property { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}