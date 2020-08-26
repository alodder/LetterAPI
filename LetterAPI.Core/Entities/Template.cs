using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LetterAPI.Core.Entities
{
    public class Template
    {
        public int Section { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string WebPath { get; set; }
        public int Id { get; set; }
    }
}
