using LetterAPI.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LetterUI.Models
{
    public class LetterListViewModel : PageViewModel
    {
        public List<Letter> Letters { get; set; }

        public List<Section> Sections { get; set; }

        public Section SelectedSection { get; set; }

        public List<string> SelectedLetters { get; set; }
    }
}
