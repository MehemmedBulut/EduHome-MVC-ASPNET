using Microsoft.AspNetCore.Mvc;

namespace Eduprob.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
