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
               .HasMany(p => p.Procedures)
               .WithMany(p => p.Users);

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
            });
            #endregion

            base.OnModelCreating(builder);
        }

        public virtual DbSet<Procedure> Procedures { get; set; }
    }
}
