using Newtonsoft.Json;

namespace Infrastructure.Models
{
	public class Error
	{
		[JsonProperty("error")]
		public string Title { get; set; }

		[JsonProperty("error_description")]
		public string Description { get; set; }
	}
}