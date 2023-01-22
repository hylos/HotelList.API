using System.ComponentModel.DataAnnotations;

namespace HotelList.API.Models.Users;

public class UserDto : LoginDto
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}
