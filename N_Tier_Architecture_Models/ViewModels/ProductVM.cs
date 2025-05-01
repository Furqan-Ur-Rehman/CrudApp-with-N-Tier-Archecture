using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace N_Tier_Architecture_Models.ViewModels
{
    public class ProductVM
    {
        //public Product Product { get; set; }
        public int Id { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Length Must Be Less Than 250 CHARACTERS")]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public int Price { get; set; }

        [Display(Name = "Images")]
        public string ImageUrl { get; set; } = null!;

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        //[ValidateNever]
        public Category Category { get; set; }
        //[ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IFormFile? Photo { get; set; }
    }
}
