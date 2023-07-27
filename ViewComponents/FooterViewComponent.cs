using Eduprob.DAL;
using Eduprob.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Eduprob.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;

        public FooterViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM footerVM = new FooterVM()
            {
                Bios = await _db.Bios.FirstOrDefaultAsync(),
                SocialMedias = await _db.SocialMedias.FirstOrDefaultAsync(),
            };
            return View(footerVM);
        }
    }
}
