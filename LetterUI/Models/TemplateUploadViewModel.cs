using LetterAPI.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace LetterUI.Models
{
    public class TemplateUploadViewModel : PageViewModel
    {
        public List<Letter> Templates { get; set; }
        public IFormFile JsonFile { get; set; }
    }
}
