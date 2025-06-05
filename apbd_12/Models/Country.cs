using System.ComponentModel.DataAnnotations;

namespace apbd_12.Models;

public class Country
{
    [Key]
    public int IdCountry { get; set; }
    public string Name { get; set; }

    public ICollection<CountryTrip> CountryTrips { get; set; }
}