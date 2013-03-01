namespace ScBootstrap.Tests.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Helpers;
    using Logic.Controllers;
    using Logic.Models.Forms;
    using Logic.Services;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class AccountControllerTest
    {
        [Test]
        public void LoginGetRedirectsToLogout()
        {
            var authService = new Mock<IAuthenticationService>();
            var controller = GetAccountController(authService.Object);

            var result = (RedirectToRouteResult)controller.Login("/pathinfo", true);

            authService.Verify(s => s.Logout());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteName, "Sitecore");
        }

        [Test]
        public void LoginGetShowsLogoutViewIfAuthenticated()
        {
            var authService = new Mock<IAuthenticationService>();
            authService.Setup(s => s.IsAuthenticated()).Returns(true);
            var controller = GetAccountController(authService.Object);

            var result = (ViewResult)controller.Login("/pathinfo", null);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, "Logout");
        }

        [Test]
        public void LoginGetShowsLoginForm()
        {
            var authService = new Mock<IAuthenticationService>();
            authService.Setup(s => s.IsAuthenticated()).Returns(false);
            var controller = GetAccountController(authService.Object);

            var result = (ViewResult)controller.Login("/pathinfo", null);

            Assert.IsNotNull(result);
        }

        [Test]
        public void LoginPostValidatesModel()
        {
            var authService = new Mock<IAuthenticationService>();
            var controller = GetAccountController(authService.Object);
            var invalidModel = new LoginForm {Name = "Jorge", Password = string.Empty};

            var result = (ViewResult)controller.Login(invalidModel);

            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count > 0);
        }

        [Test]
        public void LoginPostValidatesModelAndAuthorization()
        {
            var authService = new Mock<IAuthenticationService>();
            authService.Setup(s => s.Login("Test", "Fail", false)).Returns(false);
            var controller = GetAccountController(authService.Object);
            var invalidModel = new LoginForm { Name = "Test", Password = "Fail", Persistent = false };

            var result = (ViewResult)controller.Login(invalidModel);

            authService.Verify(s => s.Login("Test", "Fail", false), Times.Exactly(1));
            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count > 0);
        }

        [Test]
        public void LoginPostRedirectsOnValidModel()
        {
            var authService = new Mock<IAuthenticationService>();
            authService.Setup(s => s.Login("Test", "Pass", false)).Returns(true);
            var controller = GetAccountController(authService.Object);
            var validModel = new LoginForm { Name = "Test", Password = "Pass", Persistent = false };

            var result = (RedirectToRouteResult)controller.Login(validModel);

            authService.Verify(s => s.Login("Test", "Pass", false), Times.Exactly(1));
            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count == 0);
            Assert.AreEqual(result.RouteName, "Sitecore");
        }

        private static AccountController GetAccountController(IAuthenticationService authService)
        {
            var controller = new AccountController(authService);
            var contextBase = MockHelpers.FakeHttpContext();
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(contextBase, new RouteData()), new RouteCollection());
            return controller;
        }
    }
}
