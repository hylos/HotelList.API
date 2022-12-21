using System.ComponentModel.DataAnnotations;

namespace HotelList.API.Models.Hotel;

public abstract class BaseHotelDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Address { get; set; }
    public double? Rating { get; set; }
    [Required]
    [Range(1,int.MaxValue)]
    public int CountryId { get; set; }
}