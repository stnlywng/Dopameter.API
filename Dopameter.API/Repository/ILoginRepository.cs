using Dopameter.Common.DTOs;

namespace Dopameter.Repository;

public interface ILoginRepository
{
    Task<LoginSuccessResponse> GetUserByEmail(LoginRequest loginRequest);
    Task<LoginSuccessResponse> GetUserByUsername(LoginRequest loginRequest);
    Task<LoginSuccessResponse> CreateUser(SignUpRequest createUserRequest);
    Task<LoginSuccessResponse> UpdateUser(UpdateUserRequest createUserRequest);
}
