using Bilicra_Backend_Project.Config;
using Bilicra_Backend_Project.DTO;
using Bilicra_Backend_Project.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bilicra_Backend_Project.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationContext _context;

        public ProductService(ApplicationContext context)
        {
            _context = context;
        }

        public List<ProductDTO> findAll()
        {
            return findAllByParameters(null, null);
        }

        public List<ProductDTO> findAllByParameters(string name, string code)
        {
            return _context.Products
                .Where(product => !product.IsDeleted
                && (name != null ? product.Name.Contains(name) : true)
                && (code != null ? product.Code.Contains(code) : true))
                .Select(p => new ProductDTO()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code,
                    Price = p.Price,
                    Photo = p.Photo != null ? Convert.ToBase64String(p.Photo) : null,
                    LastUpdated = p.LastUpdated,
                    IsPriceConfirmed = p.IsPriceConfirmed
                })
                .ToList();
        }

        public Product findById(string id)
        {
            return _context.Products.Where(pro => !pro.IsDeleted && pro.Id == id).FirstOrDefault();
        }

        public Product create(ProductDTO dto)
        {
            Product product = new Product();
            if (_context.Products.Any(pro => pro.Code == dto.Code))
            {
                throw new Exception();
            }
            product.Id = Guid.NewGuid().ToString();
            product.Code = dto.Code;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Photo = dto.Photo != null ? Convert.FromBase64String(dto.Photo) : null;
            product.LastUpdated = DateTime.Now;
            product.IsPriceConfirmed = dto.Price < 1000;

            
            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public Product update(ProductDTO dto)
        {
            Product product = findById(dto.Id);
            if(product == null)
            {
                throw new Exception();
            }

            product.Code = dto.Code;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Photo = dto.Photo != null ? Convert.FromBase64String(dto.Photo) : null;
            product.LastUpdated = DateTime.Now;
            product.IsPriceConfirmed = dto.Price < 1000 || dto.IsPriceConfirmed;

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return product;
        }

        public void delete(string id)
        {
            Product product = _context.Products.Where(pro => !pro.IsDeleted && id.Equals(pro.Id)).FirstOrDefault();
            if (product == null)
            {
                throw new Exception();
            }

            product.IsDeleted = true;


            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public byte[] exportToExcel()
        {
            List<ProductDTO> products = findAll();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Code";
                worksheet.Cell(currentRow, 3).Value = "Name";
                worksheet.Cell(currentRow, 4).Value = "Price";
                worksheet.Cell(currentRow, 5).Value = "Photo";
                worksheet.Cell(currentRow, 6).Value = "LastUpdated";
                worksheet.Cell(currentRow, 7).Value = "IsPriceConfirmed";
                foreach (var product in products)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = product.Id;
                    worksheet.Cell(currentRow, 2).Value = product.Code;
                    worksheet.Cell(currentRow, 3).Value = product.Name;
                    worksheet.Cell(currentRow, 4).Value = product.Price;
                    worksheet.Cell(currentRow, 5).Value = product.Photo == null ? "N/A" : String.Format("data:image/png;base64,{0}", product.Photo);
                    worksheet.Cell(currentRow, 6).Value = product.LastUpdated;
                    worksheet.Cell(currentRow, 7).Value = product.IsPriceConfirmed;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return content;
                }
            }
        }
    }
}
