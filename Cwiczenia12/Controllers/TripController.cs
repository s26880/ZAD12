using Cwiczenia12.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia12.Controllers;

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