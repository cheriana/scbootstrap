namespace ScBootstrap.Tests.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Helpers;
    using Logic.Controllers;
    using Logic.Models.Domain;
    using Logic.Services;
    using Moq;
    using NUnit.Framework;
    using Logic;

    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void CanGetCarousel()
        {
            var commonService = new Mock<ICommonService>();
            var spotlights = new List<Spotlight>
            {
                new Spotlight {Id = Guid.NewGuid(), Title = "Title one"},
                new Spotlight {Id = Guid.NewGuid(), Title = "Title two"}
            };

            commonService.Setup(s => s.GetSpotlights(Constants.CarouselFolder)).Returns(spotlights);
            var controller = GetHomeController(commonService.Object);

            var result = (ViewResult)controller.Carousel();

            commonService.Verify(s => s.GetSpotlights(Constants.CarouselFolder));
            Assert.IsNotNull(result);
            Assert.AreEqual(((IList)result.Model).Count, 2);
            Assert.AreEqual(((IList<Spotlight>)result.Model)[0].Title, "Title one");
        }

        [Test]
        public void CanGetSpotlights()
        {
            var commonService = new Mock<ICommonService>();
            var spotlights = new List<Spotlight>
            {
                new Spotlight {Id = Guid.NewGuid(), Title = "Title one"},
                new Spotlight {Id = Guid.NewGuid(), Title = "Title two"}
            };

            commonService.Setup(s => s.GetSpotlights(Constants.SpotlightsFolder)).Returns(spotlights);
            var controller = GetHomeController(commonService.Object);

            var result = (ViewResult)controller.Spotlights();

            commonService.Verify(s => s.GetSpotlights(Constants.SpotlightsFolder));
            Assert.IsNotNull(result);
            Assert.AreEqual(((IList)result.Model).Count, 2);
            Assert.AreEqual(((IList<Spotlight>)result.Model)[1].Title, "Title two");
        }

        [Test]
        public void SpotlightShouldShowThreeItems()
        {
            var commonService = new Mock<ICommonService>();
            var spotlights = new List<Spotlight>
            {
                new Spotlight {Id = Guid.NewGuid(), Title = "Title one"},
                new Spotlight {Id = Guid.NewGuid(), Title = "Title two"},
                new Spotlight {Id = Guid.NewGuid(), Title = "Title three"},
                new Spotlight {Id = Guid.NewGuid(), Title = "Title four"}

            };

            commonService.Setup(s => s.GetSpotlights(Constants.SpotlightsFolder)).Returns(spotlights);
            var controller = GetHomeController(commonService.Object);

            var result = (ViewResult)controller.Spotlights();

            commonService.Verify(s => s.GetSpotlights(Constants.SpotlightsFolder));
            Assert.IsNotNull(result);
            Assert.AreEqual(((IList)result.Model).Count, 3);
        }

        private static HomeController GetHomeController(ICommonService commonService)
        {
            var controller = new HomeController(commonService);
            var contextBase = MockHelpers.FakeHttpContext();
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(contextBase, new RouteData()), new RouteCollection());
            return controller;
        }
    }
}
