using apbd_12.Data;
using apbd_12.DTO;
using apbd_12.Models;
using apbd_12.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_12.Controller;

[ApiController]
[Route("api/trips")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;
    public TripsController(ITripService tripService) => _tripService = tripService;

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _tripService.GetTripsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] Client newClient)
    {
        var success = await _tripService.AssignClientToTripAsync(newClient, idTrip, null);
        if (!success)
            return Conflict("Client with given PESEL already exists or the trip is in the past.");

        return Ok();
    }
}