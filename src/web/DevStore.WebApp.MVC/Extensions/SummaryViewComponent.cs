using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DevStore.WebApp.MVC.Extensions {
	public class SummaryViewComponent : ViewComponent {
		public IViewComponentResult Invoke() {
			return View();
		}
	}
}