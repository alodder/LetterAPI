using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetterAPI.Models
{
    public class PrintData
    {
        [Key]
        public int Pkey { get; set; }

        public DateTime PrintDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string LetterData { get; set; }
        public int Section { get; set; }

        [MaxLength(20)]
        public string LetterCode { get; set; }

        [MaxLength(10)]
        public string ActionCode { get; set; }

        [MaxLength(50)]
        public string ActionCode_Comments { get; set; }

        [MaxLength(10)]
        public string ActionCode_Dollar { get; set; }

        [MaxLength(30)]
        public string Template { get; set; }

        [MaxLength(10)]
        public string Questys { get; set; }

        [MaxLength(20)]
        public string Record { get; set; }

        [MaxLength(50)]
        public string LetterName { get; set; }

        [MaxLength(10)]
        public string UserId { get; set; }

        public string Barcode { get; set; }
        public int PrintCopies { get; set; }
    }
}
