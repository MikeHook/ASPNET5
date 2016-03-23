using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    using Microsoft.Data.Entity;
    using Microsoft.Extensions.Logging;

    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext _worldContext;
        private readonly ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext worldContext, ILogger<WorldRepository> logger)
        {
            _worldContext = worldContext;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return _worldContext.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                throw;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            return _worldContext.Trips
                .Include(i => i.Stops)
                .OrderBy(t => t.Name).ToList();
        }

        public void AddTrip(Trip newTrip)
        {
            _worldContext.Add(newTrip);
        }

        public bool SaveAll()
        {
            return _worldContext.SaveChanges() > 0;
        }

        public Trip GetTripByName(string tripName)
        {
            return _worldContext.Trips
                .Include(t => t.Stops)
                .FirstOrDefault(t => t.Name == tripName);
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var theTrip = GetTripByName(tripName);
            newStop.Order = theTrip.Stops.Max(s => s.Order) + 1;
            theTrip.Stops.Add(newStop);
            _worldContext.Stops.Add(newStop);
        }
    }
}
