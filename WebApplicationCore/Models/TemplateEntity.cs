using System.ComponentModel.DataAnnotations;

namespace WebApplicationCore.Models
{
    //This is simple POCO that represents your template that is stored in database
    public class TemplateEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(75)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
