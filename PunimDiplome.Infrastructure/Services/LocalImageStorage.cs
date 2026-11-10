using PunimDiplome.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunimDiplome.Infrastructure.Services
{
    public class LocalImageStorage : IImageStorage
    {
        private readonly string _folder;

        public LocalImageStorage(IWebHostEnvironment environment)
        {
            _folder = Path.Combine(environment.WebRootPath, "Produktet");
        }

        public async Task<string> SaveAsync(Stream content, string originalFileName)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(originalFileName);
            var fullPath = Path.Combine(_folder, fileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await content.CopyToAsync(fileStream);

            return fileName;
        }

        public void Delete(string fileName)
        {
            var fullPath = Path.Combine(_folder, fileName);
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
