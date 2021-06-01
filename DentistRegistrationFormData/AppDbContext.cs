using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentistRegistrationFormData
{
    public class AppDbContext : IdentityDbContext<User, Role, int>
    {

        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region User
            builder.Entity<User>(entity =>
           {
               entity
               .Property(p => p.Name)
               .HasMaxLength(50)
               .IsRequired(true);

               entity
               .HasIndex(p => new { p.Name, p.PhoneNumber })
               .IsUnique();

               entity
               .HasMany(p => p.ClientBookings)
               .WithOne(p => p.Client)
               .HasForeignKey(p => p.ClientId)
               .OnDelete(DeleteBehavior.Restrict);

               entity
              .HasMany(p => p.DoctorBookings)
              .WithOne(p => p.Doctor)
              .HasForeignKey(p => p.DoctorId)
              .OnDelete(DeleteBehavior.Restrict);



           });

            #endregion

            #region Procedure
            builder.Entity<Procedure>(entity =>
            {
                entity
                .Property(p => p.Name)
                .HasMaxLength(50)                
                .IsRequired(true);

                entity
                .HasIndex(p => new { p.Name })
                .IsUnique();

                entity
                .HasMany(p => p.Bookings)
                .WithOne(p => p.Procedure)
                .HasForeignKey(p => p.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region Booking
            builder.Entity<Booking>(entity =>
            {
                entity
                .HasIndex(p => new { p.dateTime, p.DoctorId, p.ProcedureId })
                .IsUnique(false);
            });
            #endregion
            base.OnModelCreating(builder);
        }

        public virtual DbSet<Procedure> Procedures { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
    }
}
