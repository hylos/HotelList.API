using AutoMapper;
using HotelList.API.Data;
using HotelList.API.Models.Country;
using HotelList.API.Models.Hotel;

namespace HotelList.API.Configurations;

    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
        }
    }

