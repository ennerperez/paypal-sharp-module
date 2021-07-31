using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infrastructure.Models
{
	public class Name
	{
		[JsonProperty("given_name")]
		public string GivenName { get; set; }

		[JsonProperty("surname")]
		public string Surname { get; set; }
	}

	public class Subscriber
	{
		[JsonProperty("name")]
		public Name Name { get; set; }

		[JsonProperty("email_address")]
		public string EmailAddress { get; set; }
	}

	public class PaymentMethod
	{
		[JsonProperty("payer_selected")]
		public string PayerSelected { get; set; }

		[JsonProperty("payee_preferred")]
		public string PayeePreferred { get; set; }
	}

	public class ApplicationContext
	{
		[JsonProperty("brand_name")]
		public string BrandName { get; set; }

		[JsonProperty("locale")]
		public string Locale { get; set; }

		[JsonProperty("shipping_preference")]
		public string ShippingPreference { get; set; }

		[JsonProperty("user_action")]
		public string UserAction { get; set; }

		[JsonProperty("payment_method")]
		public PaymentMethod PaymentMethod { get; set; }

		[JsonProperty("return_url")]
		public string ReturnUrl { get; set; }

		[JsonProperty("cancel_url")]
		public string CancelUrl { get; set; }
	}
	
	public class Link
	{
		[JsonProperty("href")]
		public string Href { get; set; }

		[JsonProperty("rel")]
		public string Rel { get; set; }

		[JsonProperty("method")]
		public string Method { get; set; }
	}

	public class Subscription
	{
		[JsonProperty("plan_id")]
		public string PlanId { get; set; }

		[JsonProperty("start_time")]
		public DateTime? StartTime { get; set; }

		[JsonProperty("subscriber")]
		public Subscriber Subscriber { get; set; }

		[JsonProperty("application_context")]
		public ApplicationContext ApplicationContext { get; set; }
		
		[JsonProperty("links")]
		public List<Link> Links { get; set; }
	}
}