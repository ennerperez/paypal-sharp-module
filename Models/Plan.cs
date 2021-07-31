using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayPal.Models
{
	public class Frequency
	{
		[JsonProperty("interval_unit")]
		public string IntervalUnit { get; set; }

		[JsonProperty("interval_count")]
		public int IntervalCount { get; set; }
	}

	public class FixedPrice
	{
		[JsonProperty("value")]
		public decimal Value { get; set; }

		[JsonProperty("currency_code")]
		public string CurrencyCode { get; set; }
	}

	public class PricingScheme
	{
		[JsonProperty("fixed_price")]
		public FixedPrice FixedPrice { get; set; }
	}

	public class BillingCycle
	{
		[JsonProperty("frequency")]
		public Frequency Frequency { get; set; }

		[JsonProperty("tenure_type")]
		public string TenureType { get; set; }

		[JsonProperty("sequence")]
		public int Sequence { get; set; }

		[JsonProperty("total_cycles")]
		public int TotalCycles { get; set; }

		[JsonProperty("pricing_scheme")]
		public PricingScheme PricingScheme { get; set; }
	}

	public class SetupFee
	{
		[JsonProperty("value")]
		public decimal Value { get; set; }

		[JsonProperty("currency_code")]
		public string CurrencyCode { get; set; }
	}

	public class PaymentPreferences
	{
		[JsonProperty("service_type")]
		public string ServiceType { get; set; }
		
		[JsonProperty("auto_bill_outstanding")]
		public bool AutoBillOutstanding { get; set; }

		[JsonProperty("setup_fee")]
		public SetupFee SetupFee { get; set; }

		[JsonProperty("setup_fee_failure_action")]
		public string SetupFeeFailureAction { get; set; }

		[JsonProperty("payment_failure_threshold")]
		public int PaymentFailureThreshold { get; set; }
		
	}

	public class Taxes
	{
		[JsonProperty("percentage")]
		public decimal Percentage { get; set; }

		[JsonProperty("inclusive")]
		public bool Inclusive { get; set; }
	}

	public class Plan
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		
		[JsonProperty("product_id")]
		public string ProductId { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
		
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("billing_cycles")]
		public List<BillingCycle> BillingCycles { get; set; }

		[JsonProperty("payment_preferences")]
		public PaymentPreferences PaymentPreferences { get; set; }
		
		[JsonProperty("quantity_supported")]
		public bool QuantitySupported { get; set; }
		
		[JsonProperty("create_time")]
		public DateTime? CreateTime { get; set; }
		
		[JsonProperty("update_time")]
		public DateTime? UpdateTime { get; set; }

		[JsonProperty("taxes")]
		public Taxes Taxes { get; set; }
	}
}