using AspNet.Data.Identity;
using AspNet.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<ProductDetails> ProductDetails { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<ProductTag> ProductTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Details)
            .WithOne(d => d.Product)
            .HasForeignKey<ProductDetails>(e => e.ProductId);

        modelBuilder.Entity<ProductDetails>()
            .HasKey(d => d.ProductId);

        modelBuilder.Entity<ProductTag>()
            .HasKey(pt => new { pt.ProductId, pt.TagId });

        modelBuilder.Entity<ProductTag>()
            .HasOne(p => p.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(e => e.ProductId);

        modelBuilder.Entity<ProductTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(pt => pt.TagId);
    }
}