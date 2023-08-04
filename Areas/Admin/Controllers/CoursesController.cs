using Eduprob.DAL;
using Eduprob.Helpers;
using Eduprob.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Eduprob.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CoursesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _db.Courses.ToListAsync();
            return View(courses);
        }
        #region Create
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            #region Exist
            bool isExist = await _db.Courses.AnyAsync(x => x.Title == course.Title);
            if (isExist)
            {
                ModelState.AddModelError("Title", "this Course is already exist");
                return View();
            }
            #endregion
            #region SaveImage
            if (course.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select file");
                return View();
            }
            if (!course.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image file");
                return View();
            }
            if (course.Photo.IsOlderMb())
            {
                ModelState.AddModelError("Photo", "Max 1mb");
                return View();
            }


            string folder = Path.Combine(_env.WebRootPath, "img", "slider");
            course.Image = await course.Photo.SaveFileAsync(folder);
            #endregion


            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return BadRequest();
            }
            if (course.IsDeactive)
            {
                course.IsDeactive = false;
            }
            else
            {
                course.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course dbCourse = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse == null)
            {
                return BadRequest();
            }
            return View(dbCourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activity(int? id, Course course)
        {
            if (id == null)
            {
                return NotFound();
            }
           Course dbCourse = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse == null)
            {
                return BadRequest();
            }
            if (dbCourse.IsDeactive)
            {
                dbCourse.IsDeactive = false;
            }
            else
            {
                dbCourse.IsDeactive = true;
            }

            if (course.Photo != null)
            {
                if (!course.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image file");
                    return View();
                }
                if (course.Photo.IsOlderMb())
                {
                    ModelState.AddModelError("Photo", "Max 1mb");
                    return View();
                }


                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                string fullPath = Path.Combine(folder, dbCourse.Image);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                dbCourse.Image = await course.Photo.SaveFileAsync(folder);

            }

            #region Exist
            bool isExist = await _db.Courses.AnyAsync(x => x.Title == course.Title);
            if (isExist)
            {
                ModelState.AddModelError("Title", "this Course is already exist");
                return View();
            }
            #endregion

            dbCourse.Title = course.Title;
            dbCourse.Description = course.Description;
            dbCourse.Image = course.Image;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course dbCourse = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse == null)
            {
                return BadRequest();
            }

            return View(dbCourse);
        }
        #endregion
    }
}
