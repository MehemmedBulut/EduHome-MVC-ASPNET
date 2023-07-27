using Microsoft.AspNetCore.Mvc;

namespace Eduprob.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
