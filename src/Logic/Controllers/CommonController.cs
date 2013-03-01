namespace ScBootstrap.Logic.Controllers
{
    using System.Web.Mvc;
    using Services;

    public class CommonController : Controller
    {
        private readonly ICommonService commonService;

        public CommonController(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        public ActionResult Navigation(string pathInfo)
        {
            ViewBag.PathInfo = pathInfo ?? string.Empty;
            ViewBag.StartUrl = commonService.GetStartUrl();
            var model = commonService.GetNavigation();
            return View(model);
        }

        public ActionResult Breadcrumb(string pathInfo)
        {
            var path = pathInfo ?? string.Empty;
            ViewBag.PathInfo = path;
            var model = commonService.GetBreadcrumb(pathInfo);
            return View(model);
        }

        public ActionResult ContactLinks()
        {
            var model = commonService.ContactLinks(Constants.ContactLinksFolder);
            return View(model);
        }
    }
}
