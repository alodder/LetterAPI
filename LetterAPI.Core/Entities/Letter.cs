using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterAPI.Core.Entities
{
    public class Letter
    {
        public string LetterName { get; set; }
        public string SectionID { get; set; }
        public string LastEditUser { get; set; }
        public DateTime LastEditDate { get; set; }
        public string LetterContent { get; set; }
    }
}
