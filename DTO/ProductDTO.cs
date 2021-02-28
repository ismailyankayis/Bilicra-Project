using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bilicra_Backend_Project.DTO
{
    public class ProductDTO
    {

        public string Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string Photo { get; set; }

        [Required]
        [Range(Double.Epsilon, Double.MaxValue, ErrorMessage = "Price must be higher than 0!")]
        public double Price { get; set; }

        public DateTime LastUpdated { get; set; }

        public bool IsPriceConfirmed { get; set; }
    }
}
