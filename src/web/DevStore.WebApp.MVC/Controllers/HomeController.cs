using DevStore.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Controllers {
	public class HomeController : MainController {
		[Route("system-unavailable")]
		public IActionResult SystemUnavailable() {
			var modelErro = new ErrorViewModel {
				Message = "The system is temporary unavailable. It could happen in times of user overload.",
				Title = "System unavailable.",
				ErroCode = 500
			};

			return View("Error", modelErro);
		}


		[Route("error/{id:length(3,3)}")]
		public IActionResult Error(int id) {
			var modelError = new ErrorViewModel();

			if (id == 500) {
				modelError.Message = "Unfortunately, an error has happened! Try again in a few moment or contact our support.";
				modelError.Title = "An error has happened!";
				modelError.ErroCode = id;
			} else if (id == 404) {
				modelError.Message =
					"The page you are looking for doesn't exist! <br />If you think this couldn't happen contact our support";
				modelError.Title = "Ops! Page not found.";
				modelError.ErroCode = id;
			} else if (id == 403) {
				modelError.Message = "You cant do this.";
				modelError.Title = "Acess Denied";
				modelError.ErroCode = id;
			} else {
				return StatusCode(404);
			}

			return View("Error", modelError);
		}
	}
}
