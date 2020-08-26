using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LetterAPI.Models
{
    [Table("Letter")]
    public class Letter
    {
        [Key]
        public String LetterName { get; set; }
        public String SectionID { get; set; }
        public String LastEditUser { get; set; }
        public DateTime LastEditDate { get; set; }
        public String LetterContent { get; set; }
    }
}
