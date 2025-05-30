using System;
using System.Collections.Generic;

namespace DevStore.WebApp.MVC.Models {
	public class ShoppingCartViewModel {
		public decimal Total { get; set; }
		public VoucherViewModel Voucher { get; set; }
		public bool HasVoucher { get; set; }
		public decimal Discount { get; set; }
		public List<ShoppingCartItemViewModel> Items { get; set; } = new List<ShoppingCartItemViewModel>();
	}

	public class ShoppingCartItemViewModel {
		public Guid ProductId { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
	}
}