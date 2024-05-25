using Domain.Entities.Products;
using NetcodeHub.Packages.Extensions.Attributes.RequiredGuid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Response.Products
{
    public class ProductBaseDTO
    {

        [Required]
        public string Name { get; set; }


        [Required]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [NetcodeHubRequiredGuid(ErrorMessage = "Product Category is required")]
        [RegularExpression(@"^[{(]?[0-9a-fa-F]{8}-([0-9a-fa-F]{4}-){3}[0-9a-fa-F]{12}[)}]?$", ErrorMessage = "Invalid Category")]

        public Guid CategoryId { get; set; }

        [NetcodeHubRequiredGuid(ErrorMessage = "Product Location is required")]
        [RegularExpression(@"^[{(]?[0-9a-fa-F]{8}-([0-9a-fa-F]{4}-){3}[0-9a-fa-F]{12}[)}]?$", ErrorMessage = "Invalid Category")]
        public Guid LocationId { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]

        public int Quantity { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Product Image")]
        public string Base64Image { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
