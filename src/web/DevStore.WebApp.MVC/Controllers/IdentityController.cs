using DevStore.WebApp.MVC.Models;
using DevStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DevStore.WebApp.MVC.Controllers {
	[Route("users")]
	public class IdentityController : MainController {
		private readonly IAuthService _authService;

		public IdentityController(
			IAuthService authService) {
			_authService = authService;
		}

		[HttpGet]
		[Route("")]
		public IActionResult Register() {
			return View();
		}

		[HttpPost]
		[Route("")]
		public async Task<IActionResult> Register(UserRegister userRegister) {
			if (!ModelState.IsValid) return View(userRegister);

			var resposta = await _authService.Register(userRegister);

			if (ResponseHasErrors(resposta.ResponseResult)) return View(userRegister);

			await _authService.DoLogin((UserLoginResponse)resposta);

			return RedirectToAction("Index", "Catalog");
		}

		[HttpGet]
		[Route("login")]
		public IActionResult Login(string returnUrl = null) {
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl = null) {
			ViewData["ReturnUrl"] = returnUrl;
			if (!ModelState.IsValid) return View(userLogin);

			var resposta = await _authService.Login(userLogin);

			if (ResponseHasErrors(resposta.ResponseResult)) return View(userLogin);

			await _authService.DoLogin(resposta);

			if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Catalog");

			return LocalRedirect(returnUrl);
		}

		[HttpGet]
		[Route("logout")]
		public async Task<IActionResult> Logout() {
			await _authService.Logout();
			return RedirectToAction("Index", "Catalog");
		}
	}
}