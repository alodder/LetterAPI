using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterAPI.Core.Entities;

namespace LetterAPI.Models
{
    public class LetterContext : DbContext
    {
        public LetterContext(DbContextOptions<LetterContext> options) : base(options)
        {

        }

        public DbSet<Letter> Letters { get; set; }

        public DbSet<PrintData> PrintData { get; set; }

        public DbSet<Template> Templates { get; set; }
    }
}
