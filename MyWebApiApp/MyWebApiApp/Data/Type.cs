using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApiApp.Data
{
    [Table("Type")]
    public class Type
    {
        [Key]
        public int IdType { get; set; }
        [Required]
        [MaxLength(100)]
        public string NameType { get; set; }

        public virtual ICollection<Product> products { get; set; }


    }
}
