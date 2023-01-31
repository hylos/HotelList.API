using AutoMapper;
using HotelList.API.Data;
using HotelList.API.Models.Users;
using HotelList.API.Static;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelList.API.IRepository.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private User _user;

        public AuthManager(IMapper mapper, UserManager<User> userManager, IConfiguration configuration, ILogger<AuthManager> logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, SD._loginProvider,SD._refreshToken);

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, SD._loginProvider, SD._refreshToken);

            var result = await _userManager.SetAuthenticationTokenAsync(_user, SD._loginProvider, SD._refreshToken, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;

            _user = await _userManager.FindByNameAsync(username);

            if(_user == null || _user.Id != request.UserId)
            {
                return null;
            }

            var isValidRefreshtoken = await _userManager.VerifyUserTokenAsync(_user, SD._loginProvider, SD._refreshToken, request.RefreshToken);

            if (isValidRefreshtoken)
            { 
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await _userManager.UpdateSecurityStampAsync(_user);

            return null;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            //Logger
            _logger.LogInformation($"Looking for user with email {loginDto.Email}");

            //check if the user exist by using th eamil
            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
           
            if(_user == null || isValidUser == false)
            {
                _logger.LogWarning($"User with email {loginDto.Email} was not found");
                return null;
            }

            //genarate token
            var token = await GenerateToken();

            //Logger
            _logger.LogInformation($"Token genarated successfully! {DateTime.Now.ToString()}");    

            return new AuthResponseDto 
            { 
                Token = token,
                UserId = _user.Id,
                RefreshToken= await CreateRefreshToken()
            };
        }

        public async Task<IEnumerable<IdentityError>> Register(UserDto userDto)
        {
            //map dto to user object
            _user = _mapper.Map<User>(userDto);

            //get email address
            _user.UserName = userDto.Email;

            //create user
            var result = await _userManager.CreateAsync(_user, userDto.Password);

            //check if user was created and assign a role.
            if(result.Succeeded)
            {
                //assing default role.
                await _userManager.AddToRoleAsync(_user, SD.DefaultUserRole);
            }

            return result.Errors;
        }

        private async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);

            //Get a list of the user roles selected from database.
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
            //if you have roleClaims at a database level thenn you can use the following line.
            var userClaims = await _userManager.GetClaimsAsync(_user);

            var claims = new List<Claim> 
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim(SD.userid, _user.Id),
            }.Union(userClaims).Union(roleClaims);

            //Create token
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
