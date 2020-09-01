using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LetterAPI.Models;
using System.Security.Cryptography.X509Certificates;
//using NReco.PdfGenerator;
using DinkToPdf;
using System.IO;

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

        // GET api/Letters/section/3
        [HttpGet("section/{Section}")]
        public async Task<ActionResult<IEnumerable<Letter>>> GetSectionLetters(string Section)
        {
            var letters = _context.Letters.Where(x => x.SectionID == Section);

            if (letters == null)
            {
                return NotFound();
            }

            return await letters.ToListAsync<Letter>();
        }

        // GET: api/Letters/pdf/78556
        [HttpGet("pdf/{pkey}")]
        public async Task<ActionResult<PrintData>> GetPDF(int pkey)
        {
            var printout = await _context.PrintData.FindAsync(pkey);

            if (printout == null)
            {
                return NotFound();
            }
            var converter = new SynchronizedConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        //HtmlContent = printout.LetterData,
                        HtmlContent = GetHtml("C:\\temp\\template.htm"),
                        WebSettings = { DefaultEncoding = "utf-8", PrintMediaType=true}
                    }
                }
            };
            byte[] pdf = converter.Convert(doc);

            return File(pdf, "application/pdf");
        }

        private string GetHtml(string filename)
        {
            string strContents;
            using (var sr = new StreamReader(filename))
            {
                // Read the stream as a string
                strContents = sr.ReadToEnd();
            }

            return strContents;
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
                if (!LetterExists(id, letter.SectionID))
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
                if (LetterExists(letter.LetterName, letter.SectionID))
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
        
        // POST: api/Letters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<ActionResult<Letter>> PutLetters(List<Letter> letters)
        {
            foreach(Letter letter in letters)
            {
                if (!LetterExists(letter.LetterName, letter.SectionID))
                {
                    _context.Letters.Add(letter);
                } else
                {
                    _context.Entry(letter).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction("GetLetters", new {});
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

        private bool LetterExists(string id, string section)
        {
            return _context.Letters.Any(e => (e.LetterName == id) && (e.SectionID == section));
        }
    }
}
