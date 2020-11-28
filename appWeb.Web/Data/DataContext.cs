using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appWeb.Common;
using appWeb.Common.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using appWeb.Web.Data.Entities;
using appWeb.Web.Chat;

namespace appWeb.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                                          
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Friend> Friends { get; set; }

        public DbSet<CategoryProduct> CategoryProduct { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Message>()
                .HasOne<User>(u => u.Sender)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.UserId);
        }
    }
}
