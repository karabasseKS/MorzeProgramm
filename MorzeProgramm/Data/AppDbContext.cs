using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MorzeProgramm.Models;

namespace MorzeProgramm.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<LearningBlock> LearningBlocks { get; set; }
        public DbSet<Letter> Letters { get; set; }

        public DbSet<ErrorStat> ErrorStats { get; set; }

    }
}