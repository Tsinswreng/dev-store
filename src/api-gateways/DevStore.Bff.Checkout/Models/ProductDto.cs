using System;

namespace DevStore.Bff.Checkout.Models {
	public class ProductDto {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
		public int Stock { get; set; }
	}
}