using HotelList.API.Data;

namespace HotelList.API.IRepository;

public interface ICountriesRepository : IGenericRepository<Country>
{
    Task<Country> GetDetails(int id);
}
