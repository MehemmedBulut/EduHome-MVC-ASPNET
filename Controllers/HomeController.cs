using Eduprob.DAL;
using Eduprob.Models;
using Eduprob.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Eduprob.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = await _db.Sliders.Where(x => !x.IsDeactive).ToListAsync(),
                Comments = await _db.Comments.ToListAsync(),
                Courses = await _db.Courses.Take(3).ToListAsync(),
                Services = await _db.Services.Where(x=>!x.IsDeactive).Take(3).ToListAsync(),
                Blogs = await _db.Blogs.Take(6).ToListAsync(),
            };

            return View(homeVM);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
