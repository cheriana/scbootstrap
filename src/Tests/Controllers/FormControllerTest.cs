namespace ScBootstrap.Tests.Controllers
{
    using System;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Helpers;
    using Logic;
    using Logic.Controllers;
    using Logic.Extensions;
    using Logic.Models.Forms;
    using Logic.Services;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class FormControllerTest
    {
        [Test]
        public void FormGetReturnsView()
        {
            var mailService = new Mock<IMailService>();
            var controller = GetFormController(mailService.Object);

            var result = (ViewResult)controller.Index(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, string.Empty);
        }

        [Test]
        public void FormGetReturnsThankyouView()
        {
            var mailService = new Mock<IMailService>();
            var controller = GetFormController(mailService.Object);

            var result = (ViewResult)controller.Index(true);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.ViewName, "Thankyou");
        }

        [Test]
        public void FormPostValidatesModel()
        {
            var mailService = new Mock<IMailService>();
            var controller = GetFormController(mailService.Object);
            controller.ModelState.AddModelError(string.Empty, string.Empty);
            var invalidModel = new ContactForm { Name = "Jorge", Email = "blah", Enquiry = string.Empty };

            var result = (ViewResult)controller.Index(invalidModel);

            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count > 0);
        }

        [Test]
        public void FormPostRedirectsOnValidModel()
        {
            var mailService = new Mock<IMailService>();
            mailService.Setup(s => s.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var model = new ContactForm { Name = "Test", Email = "test@test.com", AddressLine1 = "Line 1", AddressLine2 = "Line 2", Enquiry = "Test" };
            var controller = GetFormController(mailService.Object);

            var result = (RedirectToRouteResult)controller.Index(model);

            mailService.Verify(s => s.GetHtmlFromTemplate(It.IsAny<string>(), It.IsAny<ListDictionary>()), Times.Exactly(4));
            mailService.Verify(s => s.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count == 0);
            Assert.AreEqual(result.RouteName, "Sitecore");
            Assert.AreEqual(result.RouteValues["submit"], true);
        }

        [Test]
        public void FormFailsWhenCannotSendEmail()
        {
            var mailService = new Mock<IMailService>();
            mailService.Setup(s => s.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var model = new ContactForm { Name = "Test", Email = "test@test.com", AddressLine1 = "Line 1", AddressLine2 = "Line 2", Enquiry = "Test" };
            var controller = GetFormController(mailService.Object);

            var result = (ViewResult)controller.Index(model);

            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count == 0);
            Assert.AreEqual(result.ViewName, "Fail");
        }

        private static FormController GetFormController(IMailService mailService)
        {
            var controller = new FormController(mailService);
            var contextBase = MockHelpers.FakeHttpContext();
            controller.ControllerContext = new ControllerContext(contextBase, new RouteData(), controller);
            controller.Url = new UrlHelper(new RequestContext(contextBase, new RouteData()), new RouteCollection());
            return controller;
        }
    }
}
