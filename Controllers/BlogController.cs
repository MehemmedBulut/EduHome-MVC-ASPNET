using Microsoft.AspNetCore.Mvc;

namespace Eduprob.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
