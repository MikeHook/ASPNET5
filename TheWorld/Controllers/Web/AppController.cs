namespace TheWorld.Controllers.Web
{
    using System.Linq;
    using Microsoft.AspNet.Mvc;
    using Models;
    using Services;
    using ViewModels;

    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IWorldRepository _worldRepository;

        public AppController(IMailService mailService, IWorldRepository worldRepository)
        {
            _mailService = mailService;
            _worldRepository = worldRepository;
        }

        public IActionResult Index()
        {
            var trips = _worldRepository.GetAllTrips();
            return View(trips);
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
