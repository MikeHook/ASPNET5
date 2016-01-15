namespace TheWorld.Controllers.Web
{
    using Microsoft.AspNet.Mvc;
    using Services;
    using ViewModels;

    public class AppController : Controller
    {
        private readonly IMailService _mailService;

        public AppController(IMailService mailService)
        {
            _mailService = mailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = Startup.Configuration["AppSettings:SiteEmailAddress"];
                _mailService.SendMail(email,
                    email,
                    $"Contact from {model.Name} ({model.Email})",
                    model.Message);
            }
            return View();
        }
    }
}
