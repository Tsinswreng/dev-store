using System.Collections.Generic;

namespace DevStore.Bff.Checkout.Models {
	public class ShoppingCartDto {
		public decimal Total { get; set; }
		public VoucherDTO Voucher { get; set; }
		public bool HasVoucher { get; set; }
		public decimal Discount { get; set; }
		public List<ShoppingCartItemDto> Items { get; set; } = new List<ShoppingCartItemDto>();
	}
}