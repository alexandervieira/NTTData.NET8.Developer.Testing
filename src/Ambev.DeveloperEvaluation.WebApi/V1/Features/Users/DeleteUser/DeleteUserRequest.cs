namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Users.DeleteUser;

/// <summary>
/// Request model for deleting a user
/// </summary>
public class DeleteUserRequest
{
    /// <summary>
    /// The unique identifier of the user to delete
    /// </summary>
    public Guid Id { get; set; }
}
