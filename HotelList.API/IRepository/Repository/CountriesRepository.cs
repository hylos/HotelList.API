using HotelList.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelList.API.IRepository.Repository
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly HotelListDbContext _context;

        public CountriesRepository(HotelListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Country> GetDetails(int id)
        {
            return await _context.Countries.Include(q=> q.Hotels).FirstOrDefaultAsync(q=>q.Id== id);
        }
    }
}
