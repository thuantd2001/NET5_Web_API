using System.ComponentModel.DataAnnotations;

namespace MyWebApiApp.Models
{
    public class TypeModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
