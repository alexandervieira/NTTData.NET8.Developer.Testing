using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using Ambev.DeveloperEvoluation.Security.Models;
using Ambev.DeveloperEvoluation.Security.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

            // Garantir que a role CUSTOMER exista
            var roleName = UserRole.Customer.ToString(); // "CUSTOMER"
            var roleExists = await _authService.RoleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var roleResult = await _authService.RoleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    throw new DomainException($"Falha ao criar a role {roleName}");
                }
            }

            // Adicionar Role ao usuário
            var roleResultAdd = await _authService.UserManager.AddToRoleAsync(identityUser, roleName);
            if (!roleResultAdd.Succeeded)
            {
                throw new DomainException("Falha ao adicionar a role ao usuário");
            }

            // Adicionar Claims ao usuário
            var claims = new List<Claim>
            {
                new Claim("Permissions", "Read,Write,Edit,Delete"), // Adicione as permissões necessárias aqui
                new Claim("Roles", "Admin,Customer,Manager"),
                new Claim("Role", roleName),
                new Claim("Email", request.Email),
                new Claim("Username", request.UserName ?? request.Email)
            };

            var claimsResult = await _authService.UserManager.AddClaimsAsync(identityUser, claims);
            if (!claimsResult.Succeeded)
            {
                throw new DomainException("Falha ao adicionar claims ao usuário");
            }

            // Recuperar o hash da senha gerado pelo Identity
            var passwordHash = _authService.UserManager.PasswordHasher.HashPassword(identityUser, request.Password);

            // Remover máscara do telefone, se fornecido
            var phoneWithoutMask = request.Phone?.Replace("(", "")
                                                 .Replace(")", "")
                                                 .Replace("-", "")
                                                 .Replace(" ", "");

            // Criação do usuário na aplicação
            var appUser = new User
            {
                Id = Guid.Parse(await _authService.UserManager.GetUserIdAsync(identityUser)),
                Email = request.Email,
                Password = passwordHash, // Armazena o hash da senha no banco de dados da aplicação
                Username = request.UserName ?? request.Email,
                Phone = phoneWithoutMask,
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
            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "Logout request cannot be null");
                }

                if(request.LoggedOut == false)
                {
                    throw new UnauthorizedAccessException("User is not authenticated");
                }

                // Realiza o logout usando o SignInManager do Identity                
                return await _authService.Logout();
            }
            catch (Exception ex)
            {
                // Log de erro (se necessário)
                throw new UnauthorizedAccessException("Logout failed", ex);
            }

        }
    }

}
