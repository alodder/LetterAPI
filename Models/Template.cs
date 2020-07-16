using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LetterAPI.Models
{
    [Table("TEMPLATEDATA")]
    public class Template
    {
        public int Section { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string WebPath { get; set; }
    }
}
