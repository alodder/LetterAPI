using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LetterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly LetterContext _context;

        private readonly ILogger<SectionsController> _logger;

        public SectionsController(ILogger<SectionsController> logger, LetterContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Section>>> Get()
        {
            return await _context.Sections.ToListAsync();
        }
    }
}
