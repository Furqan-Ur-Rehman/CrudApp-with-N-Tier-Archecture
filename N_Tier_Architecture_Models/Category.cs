using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace N_Tier_Architecture_Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Name should be within 100 Characters!")]
        [DisplayName("Category Name:")]
        public string Name { get; set; }
    }
}
