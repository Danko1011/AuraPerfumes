using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuraPerfumes.Models
{
    
        [Table("Gender")]
        public class Gender
        {
            public int Id { get; set; }
            [Required]
            [MaxLength(40)]
            public string GenderLabel { get; set; }
        public List<Perfume> Perfumes { get; set; }
        }
    
}
