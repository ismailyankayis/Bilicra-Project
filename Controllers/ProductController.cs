using Bilicra_Backend_Project.Config;
using Bilicra_Backend_Project.DTO;
using Bilicra_Backend_Project.Models;
using Bilicra_Backend_Project.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bilicra_Backend_Project.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ProductController : Controller
    {

        private readonly ApplicationContext _context;
        private readonly IProductService productService;

        public ProductController(ApplicationContext context, IProductService productService)
        {
            _context = context;
            this.productService = productService;
        }

        [HttpGet]
        public ActionResult<List<ProductDTO>> getAll([FromQuery] string name, [FromQuery] string code)
        {
            return Ok(productService.findAllByParameters(name, code));
        }


        [HttpPost]
        public ActionResult<Product> create(ProductDTO dto)
        {
            try
            {
                return Ok(productService.create(dto));
            }
            catch
            {
                return BadRequest("Product code must be unique!");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Product> update(string id, ProductDTO dto)
        {
            if (!id.Equals(dto.Id))
            {
                return BadRequest();
            }  

            try
            {
                return Ok(productService.update(dto));
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult delete(string id)
        {
            try
            {
                productService.delete(id);
            }
            catch (Exception ex) when (ex is DbUpdateConcurrencyException || ex is DbUpdateException)
            {
                return BadRequest("Error while updating database.");
            }
            catch
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("export")]
        public IActionResult exportToExcel()
        {
            return File(
                        productService.exportToExcel(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "products.xlsx");
        }
    }
}
