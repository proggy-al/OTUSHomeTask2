using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;
using Otus.Teaching.PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Data
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<PromoCode> PromoCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // связь PromoCode и Customer (1 ко многим).
            modelBuilder.Entity<PromoCode>()
                .HasOne(e => e.Customer)
                .WithMany(p => p.PromoCodes)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerPreference>().HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

            // связь Customer и CustomerPreference (1 ко многим).
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(c => c.Customer)
                .WithMany(b=>b.CustomerPreferences)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // связь Preference и CustomerPreference (1 ко многим).
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(p => p.Preference)
                .WithMany(b=>b.Customers)               
                .HasForeignKey(p => p.PreferenceId);

            // пример ограничения длины для строковых полей.
            modelBuilder.Entity<Employee>().Property(e => e.FirstName).HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(e => e.LastName).HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(e => e.Email).HasMaxLength(50);

            modelBuilder.Entity<Role>().Property(e => e.Name).HasMaxLength(30);
            modelBuilder.Entity<Role>().Property(e => e.Name).IsRequired();
            modelBuilder.Entity<Role>().Property(e => e.Description).HasMaxLength(100);

            modelBuilder.Entity<Customer>().Property(e => e.FirstName).HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(e => e.LastName).HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(e => e.Email).HasMaxLength(50);

            modelBuilder.Entity<Preference>().Property(e => e.Name).HasMaxLength(50);
            modelBuilder.Entity<Preference>().Property(e => e.Name).IsRequired();

            modelBuilder.Entity<PromoCode>().Property(e => e.Code).HasMaxLength(10);
            modelBuilder.Entity<PromoCode>().Property(e => e.Code).IsRequired();
            modelBuilder.Entity<PromoCode>().Property(e => e.ServiceInfo).HasMaxLength(20);
        }
    }
}
