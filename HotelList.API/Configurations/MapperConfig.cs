using AutoMapper;
using HotelList.API.Data;
using HotelList.API.Models.Country;
using HotelList.API.Models.Hotel;
using HotelList.API.Models.Users;

namespace HotelList.API.Configurations;

    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //mapping Hotels Dto's
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Country, GetCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, UpdateCountryDto>().ReverseMap();

            //mapping Hotels Dto's
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();

            //mapping AuthUserManager Dto's
            CreateMap<UserDto, User>().ReverseMap();
        }
    }

