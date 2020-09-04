using LetterAPI.Core.Entities;
using System;
using System.Collections.Generic;

namespace LetterUI.Models
{
    public class LetterListViewModel : PageViewModel
    {
        public List<Letter> Letters { get; set; }

        public List<Section> Sections { get; set; }

        public Section SelectSection { get; set; }

        public List<Letter> SelectLetters { get; set; }
    }
}
