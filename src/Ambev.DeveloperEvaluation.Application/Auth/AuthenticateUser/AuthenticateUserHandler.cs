using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvoluation.Security.Models;
using Ambev.DeveloperEvoluation.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserCommand, UserResponseLogin>,
                                           IRequestHandler<RegisterUserCommand, UserResponseLogin>,
                                           IRequestHandler<RefreshTokenCommand, UserResponseLogin>,
                                           IRequestHandler<LogoutCommand, bool>
    {
        private readonly IUserRepository _userRepository;       
        private readonly AuthService _authService;

        public AuthenticateUserHandler(IUserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;           
            _authService = authService;
        }

        public async Task<UserResponseLogin> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var result = await _authService.SignInManager.PasswordSignInAsync(request.Email, request.Password, false, true);           

            if (result.IsLockedOut)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var activeUserSpec = new ActiveUserSpecification();
            if (!activeUserSpec.IsSatisfiedBy(user))
            {
                throw new UnauthorizedAccessException("User is not active");
            }

            var userResponseLogin = await _authService.GenerateJwt(request.Email);

            return userResponseLogin;
          
        }

        public async Task<UserResponseLogin> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // Criação do usuário no Identity
            var identityUser = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true
            };

            var existingIdentityUser = await _authService.UserManager.FindByEmailAsync(request.Email);
            if (existingIdentityUser != null)
            {
                throw new InvalidOperationException($"User with email {request.Email} already exists");
            }

            var result = await _authService.UserManager.CreateAsync(identityUser, request.Password);

            if (!result.Succeeded)
            {
                throw new DomainException("Falha ao registrar o usuário no Identity");
            }

            // Recuperar o hash da senha gerado pelo Identity
            var passwordHash = _authService.UserManager.PasswordHasher.HashPassword(identityUser, request.Password);

            // Criação do usuário na aplicação
            var appUser = new User
            {
                Id = Guid.Parse(await _authService.UserManager.GetUserIdAsync(identityUser)),
                Email = request.Email,
                Password = passwordHash, // Armazena o hash da senha no banco de dados da aplicação
                Username = request.UserName ?? $"{request.FistName}.{request.LastName}",
                Role = UserRole.Customer, // Ajuste conforme necessário
                Status = UserStatus.Active                
            };

            var existingUser = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existingUser != null)
                throw new InvalidOperationException($"User with email {request.Email} already exists");

            var createdUser = await _userRepository.CreateAsync(appUser, cancellationToken);

            if (createdUser == null)
            {
                throw new DomainException("Falha ao registrar o usuário na aplicação");
            }

            // Geração do token JWT
            var userResponseLogin = await _authService.GenerateJwt(request.Email);

            if (!userResponseLogin.UserToken.Claims.Any(c => c.Type == "Role"))
            {
                var claimsList = userResponseLogin.UserToken.Claims.ToList();
                claimsList.Add(new UserClaim
                {
                    Type = "Role",
                    Value = appUser.Role.ToString()
                });
                userResponseLogin.UserToken.Claims = claimsList;
            }

            return userResponseLogin;

        }

        public async Task<UserResponseLogin> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if(!await _authService.RefreshTokenValid(request.RefreshToken))
            {
                throw new UnauthorizedAccessException("Refresh token invalid");
            }

            var token = await _authService.GetRefreshToken(Guid.Parse(request.RefreshToken));

            return await _authService.GenerateJwt(token.Username);
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            if (!request.LoggedOut)
            {
                throw new UnauthorizedAccessException("LoggedOut failed");                
            }

            return await _authService.Logout();          

        }
    }

}
