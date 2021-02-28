using Bilicra_Backend_Project.DTO;
using Bilicra_Backend_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bilicra_Backend_Project.Services
{
    public interface IProductService
    {
        public List<ProductDTO> findAll();
        public List<ProductDTO> findAllByParameters(string name, string code);
        public Product findById(string id);
        public Product create(ProductDTO dto);
        public Product update(ProductDTO dto);
        public void delete(string id);
        public byte[] exportToExcel();
    }
}
