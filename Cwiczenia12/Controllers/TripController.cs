using CW_10_s30071.Models;
using CW_10_s30071.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s30071.Controllers;

[ApiController]
[Route("api")]
public class TripController(IDbService dbService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTripsDetails([FromQuery] int number = 1, [FromQuery] int size = 10 )
    {
        var result = await dbService.GetTripsDetailsAsync(number, size);
        return Ok(result);
    }
}