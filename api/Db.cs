using System;

using Microsoft.EntityFrameworkCore;


namespace DB
{
    public class Db : Microsoft.EntityFrameworkCore.DbContext
    {
        public Db(DbContextOptions<Db> options) : base(options) {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Models.Customer>().HasKey(i => i.Id);
            modelBuilder.Entity<Models.Customer>()
                .HasMany(i => i.Addresses)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Models.Customer>()
                .HasMany(i => i.PhoneNumbers)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Models.AddressIndirection>().HasKey(i => i.Id);
            modelBuilder.Entity<Models.AddressIndirection>()
                .HasIndex(i => i.Address)
                .IsUnique(true);
            modelBuilder.Entity<Models.AddressIndirection>()
                .Property(i => i.Address)
                .HasMaxLength(64)
                .IsRequired(true);
            modelBuilder.Entity<Models.PhoneNumberIndirection>().HasKey(i => i.Id);
            modelBuilder.Entity<Models.PhoneNumberIndirection>()
                .HasIndex(i => i.PhoneNumber)
                .IsUnique(true);
            modelBuilder.Entity<Models.PhoneNumberIndirection>()
                .Property(i => i.PhoneNumber)
                .HasMaxLength(16)
                .IsRequired(true);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}