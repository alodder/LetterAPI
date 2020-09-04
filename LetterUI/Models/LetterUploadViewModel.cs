using LetterAPI.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace LetterUI.Models
{
    public class LetterUploadViewModel : PageViewModel
    {
        public List<Letter> Letters { get; set; }
        public IFormFile JsonFile { get; set; }
    }
}
