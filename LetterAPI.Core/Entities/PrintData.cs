using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterAPI.Core.Entities
{
    public class PrintData
    {
        public int Pkey { get; set; }

        public DateTime PrintDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string LetterData { get; set; }
        public int Section { get; set; }

        public string LetterCode { get; set; }

        public string ActionCode { get; set; }

        public string ActionCode_Comments { get; set; }

        public string ActionCode_Dollar { get; set; }

        public string Template { get; set; }

        public string Questys { get; set; }

        public string Record { get; set; }

        public string LetterName { get; set; }

        public string UserId { get; set; }

        public string Barcode { get; set; }
        public int PrintCopies { get; set; }
    }
}
