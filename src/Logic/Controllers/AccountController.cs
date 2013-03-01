namespace ScBootstrap.Logic.Controllers
{
    using System.Web.Mvc;
    using Models.Forms;
    using Services;

    public class AccountController : Controller
    {
        private readonly IAuthenticationService authService;

        public AccountController(IAuthenticationService authService)
        {
            this.authService = authService;
        }

        [HttpGet]
        public ActionResult Login(string pathInfo, bool? logout)
        {
            if (logout.HasValue && logout.Value.Equals(true))
            {
                authService.Logout();
                return RedirectToRoute("Sitecore");
            }

            return authService.IsAuthenticated() ? View("Logout") : View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginForm model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
            }

            var ok = authService.Login(model.Name, model.Password, model.Persistent);
            if (!ok)
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
            }

            return RedirectToRoute("Sitecore");
        }
    }
}
