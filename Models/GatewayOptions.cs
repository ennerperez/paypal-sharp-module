namespace PayPal.Models
{
	public class GatewayOptions
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string Url { get; set; }
		public bool Debug { get; set; }
	}
}