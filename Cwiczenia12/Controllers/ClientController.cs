using CW_10_s30071.Exceptions;
using CW_10_s30071.Models.DTOs;
using CW_10_s30071.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s30071.Controllers;

[ApiController]
[Route("api")]
public class ClientController(IDbService dbService) : ControllerBase
{
   [HttpDelete]
   [Route("clients/{id}")]
   public async Task<IActionResult> DeleteClient([FromRoute] int id)
   {
      try
      {
         var deleted = await dbService.DeleteClientAsync(id);
         return Ok(deleted);
      }
      catch (NotFoundException e)
      {
         return NotFound(e.Message);
      }
      catch (InvalidOperationException e)
      {
         return BadRequest(e.Message);
      }
   }
   
   [HttpPost]
   [Route("trips/{id}/clients")]
   public async Task<IActionResult> AddClientToTrip([FromRoute] int id, [FromBody] AddClientToTripDTO client)
   {
      try
      {
         var added = await dbService.AddClientToTripAsync(id, client);
         return Ok(added);
      }
      catch (NotFoundException e)
      {
         return NotFound(e.Message);
      }
      catch (InvalidOperationException e)
      {
         return BadRequest(e.Message);
      }
   }
}