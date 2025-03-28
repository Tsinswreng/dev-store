using Microsoft.AspNetCore.Mvc;
using DevStore.WebApp.MVC.Models;

namespace DevStore.WebApp.MVC.Extensions {
	public class PagingViewComponent : ViewComponent {
		public IViewComponentResult Invoke(IPagedList pagingModel) {
			return View(pagingModel);
		}
	}
}