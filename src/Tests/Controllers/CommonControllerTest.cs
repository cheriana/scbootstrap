

namespace ScBootstrap.Tests.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Helpers;
    using Logic;
    using Logic.Controllers;
    using Logic.Models.Common;
    using Logic.Services;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CommonControllerTest
    {
        [Test]
        public void CanGetNavigation()
        {
            var commonService = new Mock<ICommonService>();
            commonService.Setup(s => s.GetStartUrl()).Returns("/");
            var controller = GetCommonController(commonService.Object);

            var result = (ViewResult)controller.Navigation("/path");

            commonService.Verify(s => s.GetStartUrl());
            commonService.Verify(s => s.GetNavigation());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewBag.PathInfo, "/path");
            Assert.AreEqual(result.ViewBag.StartUrl, "/");
        }

        [Test]
        public void CanGetBreadcrumb()
        {
            var commonService = new Mock<ICommonService>();
            var controller = GetCommonController(commonService.Object);

            var result = (ViewResult)controller.Breadcrumb("/path");

            commonService.Verify(s => s.GetBreadcrumb("/path"));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewBag.PathInfo, "/path");
        }

        [Test]
        public void CanGetContactLinks()
        {
            var commonService = new Mock<ICommonService>();
            var controller = GetCommonController(commonService.Object);

            var result = (ViewResult)controller.ContactLinks();

            commonService.Verify(s => s.ContactLinks(Constants.ContactLinksFolder));
            Assert.IsNotNull(result);
        }

        private static CommonController GetCommonController(ICommonService commonService)
        {
            var controller = new CommonController(commonService);
            var contextBase = MockHelpers.FakeHttpContext();
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(contextBase, new RouteData()), new RouteCollection());
            return controller;
        }
    }
}
