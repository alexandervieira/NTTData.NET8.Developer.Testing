using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    public class LogoutCommand : IRequest<bool>
    {   
        public bool LoggedOut { get; set; }
    }
}
