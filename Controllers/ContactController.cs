using Microsoft.AspNetCore.Mvc;

namespace Eduprob.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
