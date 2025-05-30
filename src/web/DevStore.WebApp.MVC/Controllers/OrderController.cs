using DevStore.WebApp.MVC.Models;
using DevStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevStore.WebApp.MVC.Controllers {
	public class OrderController : MainController {
		private readonly ICustomerService _customerService;
		private readonly ICheckoutBffService _checkoutBffService;

		public OrderController(ICustomerService customerService,
			ICheckoutBffService checkoutBffService) {
			_customerService = customerService;
			_checkoutBffService = checkoutBffService;
		}

		[HttpGet]
		[Route("delivery-address")]
		public async Task<IActionResult> DeliveryAddress() {
			var shoppingCart = await _checkoutBffService.GetShoppingCart();
			if (shoppingCart.Items.Count == 0) return RedirectToAction("Index", "ShoppingCart");

			var endereco = await _customerService.GetAddress();
			var order = _checkoutBffService.MapToOrder(shoppingCart, endereco);

			return View(order);
		}

		[HttpGet]
		[Route("payment")]
		public async Task<IActionResult> Payment() {
			var shoppingCart = await _checkoutBffService.GetShoppingCart();
			if (shoppingCart.Items.Count == 0) return RedirectToAction("Index", "ShoppingCart");

			var order = _checkoutBffService.MapToOrder(shoppingCart, null);

			return View(order);
		}

		[HttpPost]
		[Route("finish-order")]
		public async Task<IActionResult> FinishOrder(TransactionViewModel transaction) {
			if (!ModelState.IsValid) return View("Payment", _checkoutBffService.MapToOrder(await _checkoutBffService.GetShoppingCart(), null));

			var retorno = await _checkoutBffService.FinishOrder(transaction);

			if (ResponseHasErrors(retorno)) {
				var shoppingCart = await _checkoutBffService.GetShoppingCart();
				if (shoppingCart.Items.Count == 0) return RedirectToAction("Index", "ShoppingCart");

				var orderMap = _checkoutBffService.MapToOrder(shoppingCart, null);
				return View("Payment", orderMap);
			}

			return RedirectToAction("OrderDone");
		}

		[HttpGet]
		[Route("order-done")]
		public async Task<IActionResult> OrderDone() {
			return View("OrderDone", await _checkoutBffService.GetLastOrder());
		}

		[HttpGet("my-orders")]
		public async Task<IActionResult> MyOrders() {
			var model = await _checkoutBffService.GetCustomersById();
			return View(model);
		}
	}
}