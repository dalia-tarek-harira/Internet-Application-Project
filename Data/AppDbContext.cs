using BookSwap.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<BookPost> BookPosts { get; set; }
        public DbSet<BorrowRequest> BorrowRequests { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ApplicationUser> applicationUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
           builder.Entity<IdentityRole>().HasData(
               
               new IdentityRole(){ Name="Admin",ConcurrencyStamp="1",NormalizedName="ADMIN"},
               new IdentityRole() { Name = "Book Owner", ConcurrencyStamp = "2", NormalizedName = "BOOK OWNER" },
               new IdentityRole() { Name = "Reader", ConcurrencyStamp = "3", NormalizedName = "READER" }

              );

        }




    }
}
