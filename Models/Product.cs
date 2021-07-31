using Newtonsoft.Json;

namespace Infrastructure.Models
{
	public class Product
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("category")]
		public string Category { get; set; }

		[JsonProperty("image_url")]
		public string ImageUrl { get; set; }

		[JsonProperty("home_url")]
		public string HomeUrl { get; set; }
	}
}