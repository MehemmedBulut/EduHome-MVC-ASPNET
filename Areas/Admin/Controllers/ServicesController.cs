using Eduprob.DAL;
using Eduprob.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Eduprob.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _db;

        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> service = await _db.Services.ToListAsync();
            return View(service);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service services)
        {
            #region Exist
            bool IsExist = await _db.Services.AnyAsync(x => x.Name == services.Name);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            #endregion
            await _db.AddAsync(services);
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
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return BadRequest();
            }

            return View(dbService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Service service)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return BadRequest();
            }
            #region Exist
            bool IsExist = await _db.Services.AnyAsync(x => x.Name == service.Name && x.Id != id);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This name is already exist");
                return View();
            }
            #endregion

            dbService.Name = service.Name;
            dbService.Description = service.Description;
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
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return BadRequest();
            }

            return View(dbService);
        }
        #endregion

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeletePost(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
        //    if (dbService == null)
        //    {
        //        return BadRequest();
        //    }
        //    dbService.IsDeactive = true;
        //    await _db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service dbService = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (dbService == null)
            {
                return BadRequest();
            }
            if (dbService.IsDeactive)
            {
                dbService.IsDeactive = false;
            }
            else
            {
                dbService.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion



    }
}
