

namespace Presentation.BlazorApp.Models.Customers;

public class Customers
{
    public string? UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Biography { get; set; }
    public string? PhoneNumber { get; set; }
    public List<Address>? Addresses { get; set; }
}


public class Address
{
    public string? StreetName { get; set; }
    public string? OptionalAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}
