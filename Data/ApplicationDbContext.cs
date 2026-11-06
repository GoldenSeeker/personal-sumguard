using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReviewDraft> ReviewDrafts { get; set; }
        public DbSet<RecallRecord> RecallRecords { get; set; }
    }
}