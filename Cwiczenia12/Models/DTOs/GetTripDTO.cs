namespace Cwiczenia12.Models.DTOs;

public class GetTripDTO
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public int MaxPeople { get; set; }

    public virtual ICollection<CountryDTO> Countries { get; set; }

    public virtual ICollection<ClientDTO> Clients { get; set; }
}

public class CountryDTO
{
    public string Name { get; set; }
}

public class ClientDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}