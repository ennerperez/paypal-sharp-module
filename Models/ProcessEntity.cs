using System;

namespace PayPal.Models
{
	public class ProcessEntity<T>
	{
		public Exception Error { get; set; }
		public T Data { get; set; }
	}
}