
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bilicra_Backend_Project.Models
{
    [Index(nameof(Code), IsUnique = true)]
    public class Product
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public byte[] Photo { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public DateTime LastUpdated { get; set; }

        public bool IsPriceConfirmed { get; set; }

        public bool IsDeleted { get; set; }
    }
}
