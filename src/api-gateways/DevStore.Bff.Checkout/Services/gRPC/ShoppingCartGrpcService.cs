using System;
using System.Threading.Tasks;
using DevStore.Bff.Checkout.Models;
using DevStore.ShoppingCart.API.Services.gRPC;

namespace DevStore.Bff.Checkout.Services.gRPC {
	public interface IShoppingCartGrpcService {
		Task<ShoppingCartDto> GetShoppingCart();
	}

	public class ShoppingCartGrpcService : IShoppingCartGrpcService {
		private readonly ShoppingCartOrders.ShoppingCartOrdersClient _shoppingCartClient;

		public ShoppingCartGrpcService(ShoppingCartOrders.ShoppingCartOrdersClient shoppingCartClient) {
			_shoppingCartClient = shoppingCartClient;
		}

		public async Task<ShoppingCartDto> GetShoppingCart() {
			var Response = await _shoppingCartClient.GetShoppingCartAsync(new GetShoppingCartRequest());
			return MapShoppingCartProtoResponseDto(Response);
		}

		private static ShoppingCartDto MapShoppingCartProtoResponseDto(CustomerShoppingCartClientResponse shoppingCartResponse) {
			var cartDto = new ShoppingCartDto {
				Total = (decimal)shoppingCartResponse.Total,
				Discount = (decimal)shoppingCartResponse.Discount,
				HasVoucher = shoppingCartResponse.Hasvoucher
			};

			if (shoppingCartResponse.Voucher != null) {
				cartDto.Voucher = new VoucherDTO {
					Code = shoppingCartResponse.Voucher.Code,
					Percentage = (decimal?)shoppingCartResponse.Voucher.Percentage,
					Discount = (decimal?)shoppingCartResponse.Voucher.Discount,
					DiscountType = shoppingCartResponse.Voucher.Discounttype
				};
			}

			foreach (var item in shoppingCartResponse.Items) {
				cartDto.Items.Add(new ShoppingCartItemDto {
					Name = item.Name,
					Image = item.Image,
					ProductId = Guid.Parse(item.Productid),
					Quantity = item.Quantity,
					Price = (decimal)item.Price
				});
			}

			return cartDto;
		}
	}
}