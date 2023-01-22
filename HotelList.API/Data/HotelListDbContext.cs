using HotelList.API.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelList.API.Data
{
    public class HotelListDbContext : IdentityDbContext<User>
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

            //seeding Identity Roles coming from a separate configuaratiion file - RoleConfiguartion
            modelBuilder.ApplyConfiguration(new RoleConfiguration());

            //seeding Countries
            modelBuilder.ApplyConfiguration(new CountryConfiguration());

            //seeding Hotel
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
        }
    }
}
