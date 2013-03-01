namespace ScBootstrap.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Helpers;
    using Logic.Controllers;
    using Logic.Models.Common;
    using Logic.Models.Domain;
    using Logic.Services;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class TextpageControllerTest
    {
        [Test]
        public void CanGetGetTextpageIndex()
        {
            var textpageService = new Mock<ITextpageService>();
            var textPage = new Textpage {Id = Guid.NewGuid(), Title = "Title", Content = "Content", Teaser = "Teaser"};
            textpageService.Setup(s => s.GetTextpage("/pathinfo")).Returns(textPage);
            var controller = GetTextpageController(textpageService.Object);

            var result = (ViewResult)controller.Index("/pathinfo");

            textpageService.Verify(s => s.GetTextpage("/pathinfo"));
            Assert.IsNotNull(result);
            Assert.AreEqual(((Textpage)result.Model).Title, "Title");
        }

        [Test]
        public void CanGetGetSideNavigation()
        {
            var textpageService = new Mock<ITextpageService>();
            var navigation = new List<Navigation>
            {
                new Navigation {MenuTitle = "Name one", Url = "/url1"},
                new Navigation {MenuTitle = "Name two", Url = "/url2"}
            };
            textpageService.Setup(s => s.GetNavigation("/url1")).Returns(navigation);
            var controller = GetTextpageController(textpageService.Object);

            var result = (ViewResult)controller.SideNavigation("/url1");

            textpageService.Verify(s => s.GetNavigation("/url1"));
            Assert.IsNotNull(result);
            Assert.AreEqual(((IList<Navigation>)result.Model)[0].MenuTitle, "Name one");
            Assert.AreEqual(((IList<Navigation>)result.Model)[1].Url, "/url2");
            Assert.AreEqual(result.ViewBag.PathInfo, "/url1");
        }


        private static TextpageController GetTextpageController(ITextpageService textpageService)
        {
            var controller = new TextpageController(textpageService);
            var contextBase = MockHelpers.FakeHttpContext();
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(contextBase, new RouteData()), new RouteCollection());
            return controller;
        }
    }
}
