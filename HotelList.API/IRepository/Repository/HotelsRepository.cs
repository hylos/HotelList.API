using HotelList.API.Data;

namespace HotelList.API.IRepository.Repository
{
    public class HotelsRepository : GenericRepository<Hotel>, IHotelsRepository
    {
        public HotelsRepository(HotelListDbContext context) : base(context)
        {
        }
    }
}
