using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Eduprob.Helpers
{
    public static class Extentions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public static bool IsOlderMb(this IFormFile file)
        {
            return file.Length / 1024 > 1024;
        }
        public static async Task<string> SaveFileAsync(this IFormFile file, string folder)
        {
            string FileName = Guid.NewGuid().ToString() + file.FileName;
            string fulPath = Path.Combine(folder, FileName);

            using (FileStream fileStream = new FileStream(fulPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return FileName;
        }
    }
}
