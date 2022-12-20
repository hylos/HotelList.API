using HotelList.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelList.API.IRepository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelListDbContext _context;

        public GenericRepository(HotelListDbContext context)
        {
            this._context = context;
        }

        //Add
        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            _context.Set<T>().Remove(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);

            return entity != null;
        }

        //retrieve a list
        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if(id is null)
            {
                return null;
            }

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);

            await _context.SaveChangesAsync();
            //will come back
            return entity;
        }
    }
}
