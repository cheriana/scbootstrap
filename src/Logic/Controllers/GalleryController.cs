namespace ScBootstrap.Logic.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using PagedList;
    using Services;

    public class GalleryController : Controller
    {
        private readonly IGalleryService galleryService;

        public GalleryController(IGalleryService galleryService)
        {
            this.galleryService = galleryService;
        }

        public ActionResult Index(string pathInfo, int? page)
        {
            var model = galleryService.GetGallery(pathInfo);
            if (model.Images.Length == 0) return View("NoImages", model);

            var pageNumber = page ?? 1;
            if (pageNumber < 0 || pageNumber > model.Images.Length) return View("NoImages", model);
            ViewBag.PageNumber = pageNumber;

            var currentImage = model.Images[pageNumber - 1];
            ViewBag.CurrentImage = currentImage;

            var onePageOfImages = model.Images.ToPagedList(pageNumber, 1);
            ViewBag.OnePageOfImages = onePageOfImages;

            var list = model.Images.ToList();
            list.RemoveRange(0, pageNumber);
            model.Images = list.Take(3).ToArray();

            return View(model);
        }

        public ActionResult Galleries(string pathInfo)
        {
            var model = galleryService.GetGalleryList(pathInfo);
            return model.Galleries.Length == 0 ? View("NoGalleries", model) : View(model);
        }
    }
}
