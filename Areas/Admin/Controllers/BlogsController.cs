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
    public class BlogsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BlogsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _db.Blogs.ToListAsync();
            return View(blogs);
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            #region Exist
            bool isExist = await _db.Blogs.AnyAsync(x => x.Title == blog.Title);
            if (isExist)
            {
                ModelState.AddModelError("Title", "this Course is already exist");
                return View();
            }
            #endregion
            #region SaveImage
            if (blog.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select file");
                return View();
            }
            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image file");
                return View();
            }
            if (blog.Photo.IsOlderMb())
            {
                ModelState.AddModelError("Photo", "Max 1mb");
                return View();
            }


            string folder = Path.Combine(_env.WebRootPath, "img", "slider");
            blog.Image = await blog.Photo.SaveFileAsync(folder);
            #endregion


            await _db.Blogs.AddAsync(blog);
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
            Blog blog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                return BadRequest();
            }
            if (blog.IsDeactive)
            {
                blog.IsDeactive = false;
            }
            else
            {
                blog.IsDeactive = true;
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
            Blog dbBlog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBlog == null)
            {
                return BadRequest();
            }
            return View(dbBlog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activity(int? id, Blog blog)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog dbBlogs = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBlogs == null)
            {
                return BadRequest();
            }
            if (dbBlogs.IsDeactive)
            {
                dbBlogs.IsDeactive = false;
            }
            else
            {
                dbBlogs.IsDeactive = true;
            }

            if (blog.Photo != null)
            {
                if (!blog.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image file");
                    return View();
                }
                if (blog.Photo.IsOlderMb())
                {
                    ModelState.AddModelError("Photo", "Max 1mb");
                    return View();
                }


                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                string fullPath = Path.Combine(folder, dbBlogs.Image);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                dbBlogs.Image = await blog.Photo.SaveFileAsync(folder);

            }



            dbBlogs.Title = blog.Title;
            dbBlogs.By = blog.By;
            dbBlogs.Image = blog.Image;
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
            Blog dbBlog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBlog == null)
            {
                return BadRequest();
            }

            return View(dbBlog);
        }
        #endregion
    }
}
