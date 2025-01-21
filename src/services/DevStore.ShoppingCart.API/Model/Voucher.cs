namespace DevStore.ShoppingCart.API.Model {
	public class Voucher {
		//#AI:折扣百分比
		public decimal? Percentage { get; set; }
		//#AI:折扣金額
		public decimal? Discount { get; set; }
		//#AI:優惠券代碼
		public string Code { get; set; }

		public DiscountType DiscountType { get; set; }
	}

	public enum DiscountType {
		//#AI:百分比折扣
		Percentage = 0,
		//#AI:固定金額折扣
		Value = 1
	}
}