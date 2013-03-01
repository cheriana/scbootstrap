namespace ScBootstrap.Logic.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Services;

    public class HomeController : Controller
    {
        private readonly ICommonService commonService;

        public HomeController(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        public ActionResult Carousel()
        {
            var model = commonService.GetSpotlights(Constants.CarouselFolder);
            return View(model);
        }

        public ActionResult Spotlights()
        {
            var model = commonService.GetSpotlights(Constants.SpotlightsFolder);
            if (model.Count > 3) model = model.Take(3).ToList().AsReadOnly();
            return View(model);
        }
    }
}
