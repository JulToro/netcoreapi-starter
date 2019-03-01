using Microsoft.EntityFrameworkCore;
using netcore.infrastructure.Entities;
using System;

namespace netcore.infrastructure
{
    public class SampleDbContext :  DbContext
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        { }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region SeedData - Datos de ejemplo 
            var names = new string[] { "Wiliam", "Sofia", "Andres", "Ismael", "Juan" };
            var lastNames = new string[] { "Ramirez", "Cano", "Ortega", "Urrutia", "Perez" };
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                modelBuilder.Entity<User>()
                    .HasData(new User()
                    {
                        Id = Guid.NewGuid(),
                        Name = names[random.Next(0, names.Length)],
                        LastName = lastNames[random.Next(0, lastNames.Length)],
                        Nit = random.Next(8000000, 15000000).ToString(),
                        BirthDay = new DateTime(random.Next(1980, 2000), random.Next(1, 12), random.Next(1, 28))
                    });

            }
            #endregion
        }

    }
}
