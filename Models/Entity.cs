using System;

namespace PayPal.Models
{
	public class Entity<T>
	{
		public Exception Error { get; set; }
		public T Data { get; set; }
	}
}