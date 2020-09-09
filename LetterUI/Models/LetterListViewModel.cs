using LetterAPI.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LetterUI.Models
{
    public class LetterListViewModel : PageViewModel
    {
        [BindProperty]
        public List<Letter> Letters { get; set; }

        public List<Section> Sections { get; set; }

        public Section SelectSection { get; set; }

        [BindProperty]
        public SelectList SelectLetters { get; set; }
    }
}
