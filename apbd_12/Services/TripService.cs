using apbd_12.Data;
using apbd_12.Models;
using Microsoft.EntityFrameworkCore;

namespace apbd_12.Services;

public class TripService : ITripService
{
    private readonly MyDbContext _context;
    public TripService(MyDbContext context) => _context = context;

    public async Task<object> GetTripsAsync(int page, int pageSize)
    {
        var totalTrips = await _context.Trips.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalTrips / pageSize);

        var trips = await _context.Trips
            .Include(t => t.CountryTrips)
            .ThenInclude(ct => ct.Country)
            .Include(t => t.ClientTrips)
            .ThenInclude(ct => ct.Client)
            .OrderByDescending(t => t.DateFrom)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new
            {
                t.Name,
                t.Description,
                t.DateFrom,
                t.DateTo,
                t.MaxPeople,
                Countries = t.CountryTrips.Select(ct => new
                {
                    ct.Country.Name
                }),
                Clients = t.ClientTrips.Select(ct => new
                {
                    ct.Client.FirstName,
                    ct.Client.LastName
                })
            })
            .ToListAsync();

        return new
        {
            pageNum = page,
            pageSize,
            allPages = totalPages,
            trips
        };
    }

    public async Task<bool> AssignClientToTripAsync(Client client, int idTrip, DateTime? paymentDate)
    {
        var exists = await _context.Clients.AnyAsync(c => c.Pesel == client.Pesel);
        if (exists) return false;

        if (!await _context.Trips.AnyAsync(t => t.IdTrip == idTrip && t.DateFrom > DateTime.Now))
            return false;

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        _context.ClientTrips.Add(new ClientTrip
        {
            IdClient = client.IdClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = paymentDate
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteClientAsync(int idClient)
    {
        var hasTrips = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == idClient);
        if (hasTrips) return false;

        var client = await _context.Clients.FindAsync(idClient);
        if (client == null) return false;

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
        return true;
    }
}