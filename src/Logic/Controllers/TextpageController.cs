namespace ScBootstrap.Logic.Controllers
{
    using System.Web.Mvc;
    using Services;

    public class TextpageController : Controller
    {
        private readonly ITextpageService textpageService;

        public TextpageController(ITextpageService textpageService)
        {
            this.textpageService = textpageService;
        }

        public ActionResult Index(string pathInfo)
        {
            var model = textpageService.GetTextpage(pathInfo ?? string.Empty);
            return View(model);
        }

        public ActionResult SideNavigation(string pathInfo)
        {
            var path = pathInfo ?? string.Empty;
            ViewBag.PathInfo = path;
            var model = textpageService.GetNavigation(path);
            return View(model);
        }
    }
}
