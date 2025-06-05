using apbd_12.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_12.Controller;

[ApiController]
[Route("api/clients")]
public class ClientsController : ControllerBase
{
    private readonly MyDbContext _context;
    public ClientsController(MyDbContext context) => _context = context;

    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClient(int idClient)
    {
        var hasTrips = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == idClient);
        if (hasTrips)
            return Conflict("Client is assigned to at least one trip.");

        var client = await _context.Clients.FindAsync(idClient);
        if (client == null) return NotFound();

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}