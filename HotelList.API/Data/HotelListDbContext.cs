using Microsoft.EntityFrameworkCore;

namespace HotelList.API.Data
{
    public class HotelListDbContext : DbContext
    {
        public HotelListDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        //used to seed data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id= 1,
                    Name = "Jamaica",
                    ShortName = "JN"
                },
                new Country
                {
                    Id= 2,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id= 3,
                    Name = "Cayman Island",
                    ShortName = "CI"
                });

            modelBuilder.Entity<Hotel>().HasData(
               new Hotel
               {
                   Id = 1,
                   Name = "Sandals Resort and Spa",
                   Address = "Negril",
                   CountryId = 1,
                   Rating = 4.5
               },
               new Hotel
               {
                   Id = 2,
                   Name = "Comfort Suites",
                   Address = "George Town",
                   CountryId = 3,
                   Rating = 4.3
               },
               new Hotel
               {
                   Id = 3,
                   Name = "Grand Palldium",
                   Address = "Nassua",
                   CountryId = 2,
                   Rating = 4
               }
           );
        }
    }
}
