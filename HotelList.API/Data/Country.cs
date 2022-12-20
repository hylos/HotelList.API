using Microsoft.Build.Framework;

namespace HotelList.API.Data
{
    public class Country
    {
        //Many to one rela or one to many rel
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual IList<Hotel> Hotels { get; set; } 
    }
}