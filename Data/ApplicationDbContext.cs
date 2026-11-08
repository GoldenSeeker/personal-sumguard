using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Personal_Sumguard.Models;

namespace Personal_Sumguard.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ReviewDraft> ReviewDrafts { get; set; }
        public DbSet<RecallRecord> RecallRecords { get; set; }
        public DbSet<ReviewShare> ReviewShares { get; set; }
        public DbSet<CustomerComment> CustomerComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<InvitationToken> InvitationTokens { get; set; }
        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
    }
}