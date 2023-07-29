using Eduprob.DAL;
using Eduprob.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduprob.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;

        public BlogController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _db.Blogs.Where(x => !x.IsDeactive).OrderByDescending(x => x.Id).ToListAsync();
            return View(blogs);
        }
    }
}
