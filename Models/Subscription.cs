using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayPal.Models
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
        // "status": "ACTIVE",
        // "status_update_time": "2020-03-22T10:43:33Z",
        
        [JsonProperty("status")]
        public string Status { get; set; }
        
		[JsonProperty("plan_id")]
		public string PlanId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("shipping_amount")]
        public ShippingAmount ShippingAmount { get; set; }

		[JsonProperty("start_time")]
		public string StartTime { get; set; }

		[JsonProperty("subscriber")]
		public Subscriber Subscriber { get; set; }

		[JsonProperty("application_context")]
		public ApplicationContext ApplicationContext { get; set; }
		
		[JsonProperty("links")]
		public List<Link> Links { get; set; }
	}

    public class ShippingAmount
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
    }
}