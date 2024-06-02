using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class CustomersDto
{
    public string? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Biography { get; set; }
    public string? PhoneNumber { get; set; }
    public List<AddressDto>? Addresses { get; set; }
}


public class AddressDto
{
    public string? StreetName { get; set; }
    public string? OptionalAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}