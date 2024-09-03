using System.Data;
using Dapper;
using Dopameter.Common.Models;
using MySql.Data.MySqlClient;

namespace Dopameter.Repository;

public interface IActivityRepository
{
    Task<Activity> GetActivityByUserIDAndName(int userId, string activityName);
    Task<IEnumerable<Activity>> GetAllActivitiesByUserID(int userId);
    Task CreateOrUpdatePastActivity(int userId, string activityName, int kindOfGremlin, int intensity);
    Task DeleteActivityIfNotAssociatedWithAnyMoreGremlins(int userId, string activityName);
}
