namespace CW_10_s30071.Models.DTOs;

public class PageDTO
{
    public int Number { get; set; }
    public int Size { get; set; }
    public int AllPages { get; set; }
    public ICollection<GetTripDTO> Trips { get; set; }
}