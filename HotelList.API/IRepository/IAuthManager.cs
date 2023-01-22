using HotelList.API.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelList.API.IRepository;

public interface IAuthManager
{
    Task<IEnumerable<IdentityError>> Register(UserDto userDto);
    Task<AuthResponseDto> Login(LoginDto loginDto);

    Task<string> CreateRefreshToken();
    Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);
}
