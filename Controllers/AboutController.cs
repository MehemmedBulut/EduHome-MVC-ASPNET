using Microsoft.AspNetCore.Mvc;

namespace Eduprob.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
