using Cwiczenia12.Data;
using Cwiczenia12.Exceptions;
using Cwiczenia12.Models;
using Cwiczenia12.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia12.Services;

public interface IDbService
{
    public Task<PageDTO> GetTripsDetailsAsync(int number = 1, int size = 10);
    public Task<int> DeleteClientAsync(int id);
    public Task<int> AddClientToTripAsync(int id, AddClientToTripDTO client);
}
public class DbService(_2019sbdContext context) : IDbService
{
    public async Task<PageDTO> GetTripsDetailsAsync(int number = 1, int size = 10)
    {
        var query = context.Trips
            .Include(trip => trip.IdCountries).ThenInclude(country => country.IdTrips)
            .Include(trip => trip.ClientTrips).ThenInclude(clientTrip => clientTrip.IdClientNavigation)
            .OrderByDescending(n => n.DateFrom)
            .AsQueryable();
        
        int queryCount = await query.CountAsync();
        int all = (int) Math.Ceiling(queryCount / (double)size);
        
        var trips = await query
            .Skip((number - 1) * size)
            .Take(size)
            .Select(trip => new GetTripDTO
            {
                Name = trip.Name,
                Description = trip.Description,
                DateFrom = trip.DateFrom,
                DateTo = trip.DateTo,
                MaxPeople = trip.MaxPeople,
                Countries = trip.IdCountries.Select(country => new CountryDTO
                {
                    Name = country.Name
                }).ToList(),
                Clients = trip.ClientTrips.Select(client => new ClientDTO
                {
                    FirstName = client.IdClientNavigation.FirstName,
                    LastName = client.IdClientNavigation.LastName
                }).ToList()
            })
            .ToListAsync();
        return new PageDTO
        {
            Number = number,
            Size = size,
            AllPages = all,
            Trips = trips
        };
    }

    public async Task<int> DeleteClientAsync(int id)
    {
        var client = await context.Clients.FindAsync(id);
        if (client == null)
        {
            throw new NotFoundException($"Client with id {id} not found");
        }
        
        bool hasAnyTrip = await context.ClientTrips.AnyAsync(ct => ct.IdClient == id);
        if (hasAnyTrip)
        {
            throw new InvalidOperationException($"Client with id {id} has trips");
        }
        
        context.Clients.Remove(client);
        await context.SaveChangesAsync();
        return id;
    }
    
    public async Task<int> AddClientToTripAsync(int id, AddClientToTripDTO clientDto)
    {
        var trip = await context.Trips.FindAsync(id);
        if (trip == null)
        {
            throw new NotFoundException($"Trip with id {id} not found");
        }

        if (trip.DateFrom <= DateTime.Now)
        {
            throw new InvalidOperationException($"Trip with id {id} has already started");
        }
        
        var existingClient = await context.Clients.FirstOrDefaultAsync(c => c.Pesel == clientDto.Pesel);
        if (existingClient != null)
        {
            bool isRegistered = await context.ClientTrips.AnyAsync(ct => ct.IdTrip == id && ct.IdClient == existingClient.IdClient);
            if (isRegistered)
                throw new InvalidOperationException($"Client with PESEL {clientDto.Pesel} is already registered for trip {id}");
        }
        else
        {
            existingClient = new Client
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                Telephone = clientDto.Telephone,
                Pesel = clientDto.Pesel
            };
            context.Clients.Add(existingClient);
            await context.SaveChangesAsync();
        }

        var clientTrip = new ClientTrip
        {
            IdClient = existingClient.IdClient,
            IdTrip = id,
            RegisteredAt = DateTime.Now,
            PaymentDate = clientDto.PaymentDate
        };

        context.ClientTrips.Add(clientTrip);
        await context.SaveChangesAsync();

        return existingClient.IdClient;
    }

}