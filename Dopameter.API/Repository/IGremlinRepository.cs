using Dopameter.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dopameter.Common.DTOs;

namespace Dopameter.Repository
{
    public interface IGremlinRepository
    {
        Task<Gremlin> GetGremlinById(int gremlinId);
        Task<IEnumerable<Gremlin>> GetCurrentGremlinsByUser(int userId);
        Task<IEnumerable<Gremlin>> GetOldGremlinsByUser(int userId);
        Task DeleteGremlin(int gremlinId);
        Task<Gremlin> UpdateGremlin(Gremlin gremlin);
        Task CreateGremlin(int userId, Gremlin gremlin);
        Task FeedGremlin(int gremlinId, int oldLastSetWeight, DateTime lastFedDate, int percentFed);
        Task SetupDemoGremlins();
    }
}