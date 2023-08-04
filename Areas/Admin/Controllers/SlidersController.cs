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
    public class SlidersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SlidersController(AppDbContext db,IWebHostEnvironment env)
        {
            _db = db;
            _env=env;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _db.Sliders.ToListAsync();
            return View(slider);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            #region Exist
            bool isExist = await _db.Sliders.AnyAsync(x => x.Title == slider.Title);
            if (isExist)
            {
                ModelState.AddModelError("Title", "this Course is already exist");
                return View();
            }
            #endregion
            #region SaveImage
            if (slider.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please select file");
                return View();
            }
            if (!slider.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please select image file");
                return View();
            }
            if (slider.Photo.IsOlderMb())
            {
                ModelState.AddModelError("Photo", "Max 1mb");
                return View();
            }


            string folder = Path.Combine(_env.WebRootPath, "img", "slider");
            slider.Image = await slider.Photo.SaveFileAsync(folder);
            #endregion


            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    


        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            return View(dbSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Slider slider)
        {
            if (id == null)
            {
                return NotFound();
            }
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }
            if (dbSlider.IsDeactive)
            {
                dbSlider.IsDeactive = false;
            }
            else
            {
                dbSlider.IsDeactive = true;
            }

            if (slider.Photo != null)
            {
                if (!slider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please select image file");
                    return View();
                }
                if (slider.Photo.IsOlderMb())
                {
                    ModelState.AddModelError("Photo", "Max 1mb");
                    return View();
                }


                string folder = Path.Combine(_env.WebRootPath, "img", "slider");
                string fullPath = Path.Combine(folder, dbSlider.Image);
                if(System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                dbSlider.Image = await slider.Photo.SaveFileAsync(folder);
                
            }



            dbSlider.Title = slider.Title;
            dbSlider.Description = slider.Description;
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
            Slider slider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (slider == null)
            {
                return BadRequest();
            }
            if (slider.IsDeactive)
            {
                slider.IsDeactive = false;
            }
            else
            {
                slider.IsDeactive = true;
            }
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
            Slider dbSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (dbSlider == null)
            {
                return BadRequest();
            }

            return View(dbSlider);
        }
        #endregion
    }
}
