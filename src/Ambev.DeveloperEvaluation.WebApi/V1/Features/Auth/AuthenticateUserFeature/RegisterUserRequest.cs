namespace Ambev.DeveloperEvaluation.WebApi.V1.Features.Auth.AuthenticateUserFeature
{
    public class RegisterUserRequest
    {
        public string FistName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? UserName { get; set; }        
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public bool Active { get; set; }
        public string Password { get; set; } = null!;
        public string ConfirmePassword { get; set; } = null!;
    }

}
