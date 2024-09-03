namespace Dopameter.Common.DTOs;

public class LoginSuccessResponse
{
    public string token { get; set; }
    public int userID { get; set; }
    public string email { get; set; }
    public string username { get; set; }
}