using System.Data;
using Dapper;
using Dopameter.Common.Models;
using MySql.Data.MySqlClient;

namespace Dopameter.Repository;

public class ActivityRepository
{
    private readonly IConfiguration _config;
    private readonly ILogger<GremlinRepository> _logger;
    
    public ActivityRepository(IConfiguration config, ILogger<GremlinRepository> logger)
    {
        _config = config;
        _logger = logger;
    }
    
    public async Task<Activity> GetActivityByUserIDAndName(int userId, string activityName)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", userId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputActivityName", activityName, DbType.String, ParameterDirection.Input);

            try
            {
                var result = await connection.QueryFirstAsync<Activity>(
                    "GetActivityByUserIDAndName", 
                    parameters, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting activity with name " + activityName + " for user with ID " + userId, ex);
                throw new Exception("Error getting activity with name " + activityName + " for user with ID " + userId, ex);
            }
        }
    }
    
    public async Task<IEnumerable<Activity>> GetAllActivitiesByUserID(int userId)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", userId, DbType.Int32, ParameterDirection.Input);

            var result = await connection.QueryAsync<Activity>(
                "GetAllActivitiesByUserID", 
                parameters, 
                commandType: CommandType.StoredProcedure);

            return result;
        }
    }
    
    public async Task CreateOrUpdatePastActivity(int userId, string activityName, int kindOfGremlin, int intensity)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", userId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputActivityName", activityName, DbType.String, ParameterDirection.Input);
            parameters.Add("inputKindOfGremlin", kindOfGremlin, DbType.Int16, ParameterDirection.Input);
            parameters.Add("inputIntensity", intensity, DbType.Int32, ParameterDirection.Input);

            await connection.ExecuteAsync(
                "CreateOrUpdatePastActivity", 
                parameters, 
                commandType: CommandType.StoredProcedure);
        }
    }
}
