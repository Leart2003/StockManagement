using OfficeOpenXml;
using PunimDiplome.Application.Interfaces;
using PunimDiplome.Domain.Entities;

namespace PunimDiplome.Infrastructure.Services
{
    public class EpplusProduktExporter : IProduktExporter
    {
        public byte[] ExportToExcel(IEnumerable<Produkt> produktet)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Produktet");

            string[] headers = { "Id", "Name", "Brand", "Description", "Ram", "Price", "Date",
                                 "Storage", "Storage Type", "Screen Inch", "Procesor", "Category", "Image File Name" };

            for (int i = 0; i < headers.Length; i++)
                worksheet.Cells[1, i + 1].Value = headers[i];

            int row = 2;
            foreach (var p in produktet)
            {
                worksheet.Cells[row, 1].Value = p.Id;
                worksheet.Cells[row, 2].Value = p.Name;
                worksheet.Cells[row, 3].Value = p.Brand;
                worksheet.Cells[row, 4].Value = p.Description;
                worksheet.Cells[row, 5].Value = p.Ram;
                worksheet.Cells[row, 6].Value = p.Price;
                worksheet.Cells[row, 7].Value = p.Date.ToString("yyyy-MM-dd");
                worksheet.Cells[row, 8].Value = p.Storage;
                worksheet.Cells[row, 9].Value = p.Storagetype;
                worksheet.Cells[row, 10].Value = p.ScreenInch;
                worksheet.Cells[row, 11].Value = p.Procesor;
                worksheet.Cells[row, 12].Value = p.Category;
                worksheet.Cells[row, 13].Value = p.ImageFileName;
                row++;
            }

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}