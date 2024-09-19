using System.Data;
using Dapper;
using Dopameter.BusinessLogic;
using Dopameter.Common.DTOs;
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
            parameters.Add("inputPleasurePain", gremlin.pleasurePain, DbType.Int16, ParameterDirection.Input);
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
            parameters.Add("inputPleasurePain", gremlin.pleasurePain, DbType.Int16, ParameterDirection.Input);
            parameters.Add("inputIntensity", gremlin.intensity, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputLastSetWeight", 100, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputDateOfBirth", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("inputLastFedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

            var result = await connection.ExecuteAsync(
                "AddNewGremlin",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

    }
    
    // Variation to not default the days to Now.
    public async Task CreateGremlinDemo(int userID, Gremlin gremlin)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", userID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputName", gremlin.name, DbType.String, ParameterDirection.Input);
            parameters.Add("inputActivityName", gremlin.activityName, DbType.String, ParameterDirection.Input);
            parameters.Add("inputKindOfGremlin", gremlin.kindOfGremlin, DbType.Int16, ParameterDirection.Input);
            parameters.Add("inputPleasurePain", gremlin.pleasurePain, DbType.Int16, ParameterDirection.Input);
            parameters.Add("inputIntensity", gremlin.intensity, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputLastSetWeight", 100, DbType.Int32, ParameterDirection.Input);
            parameters.Add("inputDateOfBirth", gremlin.dateOfBirth, DbType.DateTime, ParameterDirection.Input);
            parameters.Add("inputLastFedDate", gremlin.lastFedDate, DbType.DateTime, ParameterDirection.Input);

            var result = await connection.ExecuteAsync(
                "AddNewGremlin",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

    }
    
    public async Task FeedGremlin(int gremlinId, int oldLastSetWeight, DateTime lastFedDate, int percentFed)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var newWeight = CalculateWeight.CalculateGremlinWeight(oldLastSetWeight, lastFedDate, percentFed);
            
                var parameters = new DynamicParameters();
                parameters.Add("inputGremlinID", gremlinId, DbType.Int32, ParameterDirection.Input);
                parameters.Add("inputLastSetWeight", newWeight, DbType.Int32, ParameterDirection.Input);
                parameters.Add("inputLastFedDate", DateTime.Now, DbType.DateTime, ParameterDirection.Input);

                var result = await connection.ExecuteAsync(
                    "UpdateGremlinWeightAndFedDate",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Error feeding gremlin with ID " + gremlinId, ex);
            throw new Exception("Error feeding gremlin with ID " + gremlinId, ex);
        }
        
    }
    
    public async Task UpdateGremlinDatesDemo(int gremlinId, Gremlin gremlin)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("inputGremlinID", gremlin.gremlinID, DbType.Int32, ParameterDirection.Input);
                parameters.Add("inputLastSetWeight", gremlin.lastSetWeight, DbType.Int32, ParameterDirection.Input);
                parameters.Add("inputDateOfBirth", gremlin.dateOfBirth, DbType.DateTime, ParameterDirection.Input);
                parameters.Add("inputLastFedDate", gremlin.lastFedDate, DbType.DateTime, ParameterDirection.Input);

                var result = await connection.ExecuteAsync(
                    "UpdateGremlinDates",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
        catch(Exception ex)
        {
            _logger.LogError("Error feeding gremlin with ID " + gremlinId, ex);
            throw new Exception("Error feeding gremlin with ID " + gremlinId, ex);
        }
        
    }

    public async Task SetupDemoGremlins()
    {
        var demoGremlins = await GetCurrentGremlinsByUser(-5);

        // Create an array of gremlins
        Gremlin[] gremlinsArray = new Gremlin[]
        {
            new Gremlin
            {
                name = "Gambling George",
                activityName = "Slots and Black-Jack.",
                kindOfGremlin = 1,
                pleasurePain = 1,
                intensity = 800,
                lastSetWeight = 100,
                dateOfBirth = DateTime.Now.AddDays(-10),
                lastFedDate = DateTime.Now.AddDays(-5),
            },
            new Gremlin
            {
                name = "Doom Scroller",
                activityName = "Scrolling on TikTok.",
                kindOfGremlin = 2,
                pleasurePain = 1,
                intensity = 350,
                lastSetWeight = 100,
                dateOfBirth = DateTime.Now.AddDays(-10),
                lastFedDate = DateTime.Now.AddDays(-2)
            },
            new Gremlin
            {
                name = "Leafy",
                activityName = "Smoking.",
                kindOfGremlin = 3,
                pleasurePain = 1,
                intensity = 200,
                lastSetWeight = 100,
                dateOfBirth = DateTime.Now.AddDays(-20),
                lastFedDate = DateTime.Now.AddDays(-14)
            },
            new Gremlin
            {
                name = "Bolt",
                activityName = "Running 10K.",
                kindOfGremlin = 5,
                pleasurePain = 2,
                intensity = 400,
                lastSetWeight = 100,
                dateOfBirth = DateTime.Now.AddDays(-8),
                lastFedDate = DateTime.Now
            },
            new Gremlin
            {
                name = "Gordon Ramsay",
                activityName = "Cooking.",
                kindOfGremlin = 4,
                pleasurePain = 2,
                intensity = 80,
                lastSetWeight = 100,
                dateOfBirth = DateTime.Now,
                lastFedDate = DateTime.Now
            }
        };
        
        if (demoGremlins.Count() == 0)
        {
            foreach(var gremlin in gremlinsArray)
            {
                await CreateGremlinDemo(-5, gremlin);
            }
        }
        else
        {
            for (int i = 0; i < demoGremlins.Count(); i++)
            {
                var currentDemoGremlinID = demoGremlins.ElementAt(i).gremlinID;
                
                if (i >= 5)
                {
                    await DeleteGremlin(currentDemoGremlinID);
                    continue;
                }
                
                var gremlin = gremlinsArray[i];
            
                // Modify the gremlin data based on index or other conditions if needed
                var tempGrem = new Gremlin
                {
                    gremlinID = currentDemoGremlinID,
                    name = gremlin.name, // You can use the gremlin's name here
                    activityName = gremlin.activityName, // Use the gremlin's activity name
                    kindOfGremlin = gremlin.kindOfGremlin, // Use kind of gremlin from the array
                    pleasurePain = gremlin.pleasurePain,
                    intensity = gremlin.intensity, // Or set new intensity if needed
                    lastSetWeight = gremlin.lastSetWeight, 
                    dateOfBirth = gremlin.dateOfBirth,
                    lastFedDate = gremlin.lastFedDate
                };
            
                // Call the UpdateGremlin method for each gremlin
                await UpdateGremlin(tempGrem);
                await UpdateGremlinDatesDemo(currentDemoGremlinID, tempGrem);
            }
        }
    }
    
    // public async Task<IEnumerable<FeedHistoryRow>> GetGremlinFeedingHistory(int gremlinID)
    // {
    //     using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
    //     {
    //         var parameters = new DynamicParameters();
    //         parameters.Add("p_gremlinID", gremlinID, DbType.Int32, ParameterDirection.Input);
    //         
    //         try
    //         {
    //             var result = await connection.QueryAsync<FeedHistoryRow>(
    //                 "GetFeedingChartByGremlinID",
    //                 parameters,
    //                 commandType: CommandType.StoredProcedure);
    //
    //             return result;
    //         }
    //         catch(Exception ex)
    //         {
    //             _logger.LogError("Error getting feed history for gremlin with ID " + gremlinID, ex);
    //             return null;
    //         }
    //     }
    // }
    
}