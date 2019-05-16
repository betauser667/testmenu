using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationCore.Models
{
    public class BaseModel
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(255)]
        [Column(Order = 1)]
        public string Name { get; set; }

        [Column(Order = 2)]
        public DateTime CreatedDate { get; set; }
    }
}
