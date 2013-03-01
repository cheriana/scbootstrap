namespace ScBootstrap.Tests.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Helpers;
    using Logic.Controllers;
    using Logic.Models.Common;
    using Logic.Models.Domain;
    using Logic.Services;
    using Moq;
    using NUnit.Framework;
    using PagedList;

    [TestFixture]
    public class GalleryControllerTest
    {
        [Test]
        public void DisplaysNoGalleriesViewIfEmpty()
        {
            var galleryService = new Mock<IGalleryService>();
            var galleryList = new GalleryList { Id = Guid.NewGuid(), Title = "Title", Teaser = "Teaser", Galleries = new Gallery[0] };
            galleryService.Setup(s => s.GetGalleryList("/pathinfo")).Returns(galleryList);
            var controller = GetGalleryController(galleryService.Object);

            var result = (ViewResult)controller.Galleries("/pathinfo");

            galleryService.Verify(s => s.GetGalleryList(It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(result.ViewName, "NoGalleries");
        }

        [Test]
        public void CanDisplayGalleries()
        {
            var galleryService = new Mock<IGalleryService>();
            var galleryList = new GalleryList { Id = Guid.NewGuid(), Title = "Title", Teaser = "Teaser", Galleries = new Gallery[2] };
            galleryService.Setup(s => s.GetGalleryList("/pathinfo")).Returns(galleryList);
            var controller = GetGalleryController(galleryService.Object);

            var result = (ViewResult)controller.Galleries("/pathinfo");

            galleryService.Verify(s => s.GetGalleryList(It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(result.ViewName, string.Empty);
        }

        [Test]
        public void WontDisplayGalleryIfEmpty()
        {
            var galleryService = new Mock<IGalleryService>();
            var gallery = new Gallery { Id = Guid.NewGuid(), Url = "/url", Title = "Title", Teaser = "Teaser", Images = new Image[0] };
            galleryService.Setup(s => s.GetGallery("/pathinfo")).Returns(gallery);
            var controller = GetGalleryController(galleryService.Object);

            var result = (ViewResult)controller.Index("/pathinfo", null);

            galleryService.Verify(s => s.GetGallery(It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(result.ViewName, "NoImages");
        }

        [Test]
        public void NoGalleryIfPageNumberIsNegative()
        {
            var galleryService = new Mock<IGalleryService>();
            var gallery = new Gallery { Id = Guid.NewGuid(), Url = "/url", Title = "Title", Teaser = "Teaser", Images = new Image[3] };
            galleryService.Setup(s => s.GetGallery("/pathinfo")).Returns(gallery);
            var controller = GetGalleryController(galleryService.Object);

            var result = (ViewResult)controller.Index("/pathinfo", -2);

            galleryService.Verify(s => s.GetGallery(It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(result.ViewName, "NoImages");
        }

        [Test]
        public void NoGalleryIfPageNumberGreaterThanNumberOfImage()
        {
            var galleryService = new Mock<IGalleryService>();
            var gallery = new Gallery { Id = Guid.NewGuid(), Url = "/url", Title = "Title", Teaser = "Teaser", Images = new Image[3] };
            galleryService.Setup(s => s.GetGallery("/pathinfo")).Returns(gallery);
            var controller = GetGalleryController(galleryService.Object);

            var result = (ViewResult)controller.Index("/pathinfo", 7);

            galleryService.Verify(s => s.GetGallery(It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.AreEqual(result.ViewName, "NoImages");
        }

        [Test]
        public void CanDisplayGallery()
        {
            var galleryService = new Mock<IGalleryService>();
            var gallery = new Gallery { Id = Guid.NewGuid(), Url = "/url", Title = "Title", Teaser = "Teaser", Images = new[] { new Image(), new Image(), new Image() } };
            galleryService.Setup(s => s.GetGallery("/pathinfo")).Returns(gallery);
            var controller = GetGalleryController(galleryService.Object);

            var result = (ViewResult)controller.Index("/pathinfo", 2);

            galleryService.Verify(s => s.GetGallery(It.IsAny<string>()), Times.Once());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNotNull(result.ViewBag.PageNumber);
            Assert.AreEqual(typeof(int), result.ViewBag.PageNumber.GetType());
            Assert.IsNotNull(result.ViewBag.CurrentImage);
            Assert.AreEqual(typeof(Image), result.ViewBag.CurrentImage.GetType());
            Assert.IsNotNull(result.ViewBag.OnePageOfImages);
            Assert.AreEqual(typeof(PagedList<Image>), result.ViewBag.OnePageOfImages.GetType());
            Assert.AreEqual(result.ViewName, string.Empty);
        }

        private static GalleryController GetGalleryController(IGalleryService galleryService)
        {
            var controller = new GalleryController(galleryService);
            var contextBase = MockHelpers.FakeHttpContext();
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(contextBase, new RouteData()), new RouteCollection());
            return controller;
        }
    }
}
