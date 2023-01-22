﻿using System.ComponentModel.DataAnnotations;

namespace HotelList.API.Models.Users;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    //[DataType(DataType.Password)]
    [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 6)]
    public string Password { get; set; }
}