using System.Data;
using Dapper;
using Dopameter.Common.Models;
using MySql.Data.MySqlClient;

namespace Dopameter.Repository;

public class GremlinRepository : IGremlinRepository
{
    private readonly IConfiguration _config;
    private readonly ILogger<GremlinRepository> _logger;

    public GremlinRepository(IConfiguration config, ILogger<GremlinRepository> logger)
    {
        _config = config;
        _logger = logger;
    }
    
    public async Task<Gremlin> GetGremlinById(int gremlinId)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputGremlinID", gremlinId, DbType.Int32, ParameterDirection.Input);

            try
            {
                var result = await connection.QueryFirstAsync<Gremlin>(
                    "GetGremlinByID", 
                    parameters, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting gremlin with ID " + gremlinId, ex);
                throw new Exception("Error getting gremlin with ID " + gremlinId, ex);
            }
        }
    }
    
    public async Task<IEnumerable<Gremlin>> GetCurrentGremlinsByUser(int userId)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", userId, DbType.Int32, ParameterDirection.Input);

            var result = await connection.QueryAsync<Gremlin>(
                "GetCurrentGremlinsByUserID", 
                parameters, 
                commandType: CommandType.StoredProcedure);

            return result;
        }
    }
    
    public async Task<IEnumerable<Gremlin>> GetOldGremlinsByUser(int userId)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", userId, DbType.Int32, ParameterDirection.Input);
            
            var result = await connection.QueryAsync<Gremlin>(
                "GetOldGremlinsByUserID", 
                parameters, 
                commandType: CommandType.StoredProcedure);

            return result;
        }
    }
    
    public async Task DeleteGremlin(int gremlinId)
    {  
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputGremlinID", gremlinId, DbType.Int32, ParameterDirection.Input);
            
            var result = await connection.ExecuteAsync(
                "DeleteGremlin",
                parameters,
                commandType: CommandType.StoredProcedure);
            
        }
    }
    
    public async Task<Gremlin> UpdateGremlin(Gremlin gremlin)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputGremlinID", gremlin.gremlinID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputName", gremlin.name, DbType.String, ParameterDirection.Input);
            parameters.Add("inputActivityName", gremlin.activityName, DbType.String, ParameterDirection.Input);
            parameters.Add("inputKindOfGremlin", gremlin.kindOfGremlin, DbType.Int16, ParameterDirection.Input);
            parameters.Add("inputIntensity", gremlin.intensity, DbType.Int32, ParameterDirection.Input);
            
            try
            {
                var result = await connection.QueryFirstAsync<Gremlin>(
                    "EditGremlin",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error updating gremlin with ID " + gremlin.gremlinID, ex);
                throw new Exception("Error updating gremlin with ID " + gremlin.gremlinID, ex);
            }
        }
    }
    
    // When creating the DB needs to determine the ID. So it has a hiccup for returning upon creation.
    public async Task CreateGremlin(int userID, Gremlin gremlin)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", userID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputName", gremlin.name, DbType.String, ParameterDirection.Input);
            parameters.Add("inputActivityName", gremlin.activityName, DbType.String, ParameterDirection.Input);
            parameters.Add("inputKindOfGremlin", gremlin.kindOfGremlin, DbType.Int16, ParameterDirection.Input);
            parameters.Add("inputIntensity", gremlin.intensity, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputDateOfBirth", gremlin.dateOfBirth, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("inputLastFedDate", gremlin.lastFedDate, DbType.DateTime, ParameterDirection.Input);

            var result = await connection.ExecuteAsync(
                "AddNewGremlin",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

    }
    
    public async Task FeedGremlin(int gremlinId)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            // Run SQL query to update the last fed date of the gremlin to datetime now, 
        }
    }
    
}