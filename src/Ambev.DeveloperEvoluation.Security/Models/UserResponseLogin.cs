namespace Ambev.DeveloperEvoluation.Security.Models;
public class UserResponseLogin
{
    public string AccessToken { get; set; } = null!;
    public Guid RefreshToken { get; set; }
    public double ExpiresIn { get; set; }
    public UserToken UserToken { get; set; } = null!;
}