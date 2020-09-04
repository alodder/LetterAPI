using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LetterAPI.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        public string SectionName { get; set; }
    }
}
