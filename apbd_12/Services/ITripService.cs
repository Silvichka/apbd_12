using apbd_12.Models;

namespace apbd_12.Services;

public interface ITripService
{
    Task<object> GetTripsAsync(int page, int pageSize);
    Task<bool> AssignClientToTripAsync(Client client, int idTrip, DateTime? paymentDate);
    Task<bool> DeleteClientAsync(int idClient);
}