namespace Ambev.DeveloperEvoluation.Security.Models;
public class UserToken
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public IEnumerable<UserClaim> Claims { get; set; } = new List<UserClaim>();
}