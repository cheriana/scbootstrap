namespace ScBootstrap.Logic.Controllers
{
    using System;
    using System.Collections.Specialized;
    using System.Web.Mvc;
    using Extensions;
    using Models.Forms;
    using Services;

    public class FormController : Controller
    {
        private readonly IMailService mailService;

        public FormController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpGet]
        public ActionResult Index(bool? submit)
        {
            return submit.HasValue && submit.Value.Equals(true)
                ? View("Thankyou") 
                : View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(ContactForm model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The contact form is incorrect.");
                return View(model);
            }

            // Send email
            var now = DateTime.Now;
            var emailReplacements = new ListDictionary
            {
                { "<%Date%>", String.Format("{0:HH:mm:ss}", now) }, 
                { "<%Time%>", String.Format("{0:dd/MM/yyyy}", now) },
                { "<%Name%>", model.Name.RemoveTags() },
                { "<%AddressLine1%>", model.AddressLine1.RemoveTags() },
                { "<%AddressLine2%>", model.AddressLine2.RemoveTags() },
                { "<%Email%>", model.Email },
                { "<%Enquiry%>", model.Enquiry.RemoveTags() }
            };

            var emailHtml = mailService.GetHtmlFromTemplate(Constants.EmailHtmlTemplate, emailReplacements);
            var emailText = mailService.GetHtmlFromTemplate(Constants.EmailTextTemplate, emailReplacements);
            var okEmail = mailService.SendMail("from@test.com", "recipients@test.com", "Subject", emailHtml, emailText);
            if (!okEmail) return View("Fail");

            // Reply email
            var replyReplacements = new ListDictionary { { "<%Name%>", model.Name.RemoveTags() } };
            var replyHtml = mailService.GetHtmlFromTemplate(Constants.ReplyHtmlTemplate, replyReplacements);
            var replyText = mailService.GetHtmlFromTemplate(Constants.ReplyTextTemplate, replyReplacements);
            var okReply = mailService.SendMail("from@test.com", model.Email, "Subject", replyHtml, replyText);
            if (!okReply) return View("Fail");

            return RedirectToRoute("Sitecore", new {submit = true});
        }
    }
}
