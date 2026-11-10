using System;
using System.Collections.Generic;
using System.Text;

namespace PunimDiplome.Application.Interfaces
{
    public interface IImageStorage
    {
        Task<string> SaveAsync(Stream content, string originalFileName);
        void Delete(string fileName);
    }
}
