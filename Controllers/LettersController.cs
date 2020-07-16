using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LetterAPI.Models;

namespace LetterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LettersController : ControllerBase
    {
        private readonly LetterContext _context;

        public LettersController(LetterContext context)
        {
            _context = context;
        }

        // GET: api/Letters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Letter>>> GetLetters()
        {
            return await _context.Letters.ToListAsync();
        }

        // GET: api/Letters/5
        [HttpGet("{LetterName}")]
        public async Task<ActionResult<Letter>> GetLetter(string LetterName)
        {
            var letter = await _context.Letters.FindAsync(LetterName);

            if (letter == null)
            {
                return NotFound();
            }

            return letter;
        }

        // PUT: api/Letters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLetter(string id, Letter letter)
        {
            if (id != letter.LetterName)
            {
                return BadRequest();
            }

            _context.Entry(letter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LetterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Letters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Letter>> PostLetter(Letter letter)
        {
            _context.Letters.Add(letter);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LetterExists(letter.LetterName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLetter", new { id = letter.LetterName }, letter);
        }

        // DELETE: api/Letters/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Letter>> DeleteLetter(string id)
        {
            var letter = await _context.Letters.FindAsync(id);
            if (letter == null)
            {
                return NotFound();
            }

            _context.Letters.Remove(letter);
            await _context.SaveChangesAsync();

            return letter;
        }

        private bool LetterExists(string id)
        {
            return _context.Letters.Any(e => e.LetterName == id);
        }
    }
}
