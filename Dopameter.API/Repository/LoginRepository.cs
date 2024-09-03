using System.Data;
using Dapper;
using Dopameter.Common.DTOs;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace Dopameter.Repository;

public class LoginRepository : ILoginRepository
{
    private readonly IConfiguration _config;
    private readonly ILogger<LoginRepository> _logger;

    public LoginRepository(IConfiguration config, ILogger<LoginRepository> logger)
    {
        _config = config;
        _logger = logger;
    }
    
    public async Task<LoginSuccessResponse> GetUserByEmail(LoginRequest loginRequest)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputEmail", loginRequest.username, DbType.String, ParameterDirection.Input);
            parameters.Add("inputPassword", loginRequest.password, DbType.String, ParameterDirection.Input);

            try
            {
                var result = await connection.QueryFirstAsync<LoginSuccessResponse>(
                    "GetUserByEmail", 
                    parameters, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting user by email " + loginRequest.username, ex);
                return null;
            }
        }
    }
    
    public async Task<LoginSuccessResponse> GetUserByUsername(LoginRequest loginRequest)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUsername", loginRequest.username, DbType.String, ParameterDirection.Input);
            parameters.Add("inputPassword", loginRequest.password, DbType.String, ParameterDirection.Input);

            try
            {
                var result = await connection.QueryFirstAsync<LoginSuccessResponse>(
                    "GetUserByUsername", 
                    parameters, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting user by username " + loginRequest.username, ex);
                return null;
            }
        }
    }
    
    public async Task<LoginSuccessResponse> CreateUser(SignUpRequest createUserRequest)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputEmail", createUserRequest.email, DbType.String, ParameterDirection.Input);
            parameters.Add("inputUsername", createUserRequest.username, DbType.String, ParameterDirection.Input);
            parameters.Add("inputPassword", createUserRequest.password, DbType.String, ParameterDirection.Input);

            try
            {
                var result = await connection.QueryFirstAsync<LoginSuccessResponse>(
                    "CreateUser", 
                    parameters, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error creating user " + createUserRequest.username, ex);
                return null;
            }
        }
    }
    
    public async Task<LoginSuccessResponse> UpdateUser(UpdateUserRequest createUserRequest)
    {
        using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
        {
            var parameters = new DynamicParameters();
            parameters.Add("inputUserID", createUserRequest.userID, DbType.Int32, ParameterDirection.Input);
            parameters.Add("newEmail", createUserRequest.email, DbType.String, ParameterDirection.Input);
            parameters.Add("newUsername", createUserRequest.username, DbType.String, ParameterDirection.Input);
            parameters.Add("newPassword", createUserRequest.password, DbType.String, ParameterDirection.Input);

            try
            {
                var result = await connection.QueryFirstAsync<LoginSuccessResponse>(
                    "UpdateUser", 
                    parameters, 
                    commandType: CommandType.StoredProcedure);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error updating user " + createUserRequest.username, ex);
                return null;
            }
        }
    }
}