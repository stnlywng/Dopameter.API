namespace Dopameter.Common.DTOs;

public class UpdateUserRequest
{
    public int userID { get; set; }
    public string email { get; set; }
    public string username { get; set; }
    public string password { get; set; }
}