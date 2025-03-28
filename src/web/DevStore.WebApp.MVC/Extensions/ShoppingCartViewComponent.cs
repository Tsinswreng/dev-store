using DevStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevStore.WebApp.MVC.Extensions {
	public class ShoppingCartViewComponent : ViewComponent {
		private readonly ICheckoutBffService _checkoutBffService;

		public ShoppingCartViewComponent(ICheckoutBffService checkoutBffService) {
			_checkoutBffService = checkoutBffService;
		}

		public async Task<IViewComponentResult> InvokeAsync() {
			return View(await _checkoutBffService.GetShoppingCartItemsQuantity());
		}
	}
}