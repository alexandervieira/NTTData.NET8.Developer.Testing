using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Ambev.DeveloperEvoluation.Security.Extensions
{
    public static class RolesAndClaimsExtensions
    {
        public static async void SeedDataIdentityRolesAndClaims<TContext>(this IApplicationBuilder app, Action<TContext> seeder)
        where TContext : DbContext
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetRequiredService<TContext>();

            ArgumentNullException.ThrowIfNull(context, nameof(context));
            context.Database.Migrate();

            // Executa o seeder customizado
            seeder(context);

            // Adiciona roles e claims padrão
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var userRepo = serviceScope.ServiceProvider.GetRequiredService<IUserRepository>();

            // Criar roles padrão
            var roles = new List<string> { "Admin", "Customer", "Manager" };
            foreach (var role in roles)
            {
                if (!roleManager.Roles.Any(r => r.Name == role))
                {
                    var result = roleManager.CreateAsync(new IdentityRole(role)).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Erro ao criar a role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // Criar claims padrão
            var defaultClaims = new List<Claim>
            {               
                new Claim("Permissions", "Read,Write,Edit,Delete"),
                new Claim("Role", UserRole.Admin.ToString()),
                new Claim("Roles", "Admin,Customer,Manager"),
                new Claim("Email", "admin@dominio.com"),
                new Claim("Username", "admin@dominio.com")
            };

            // Associar claims à role "Admin"
            var adminRole = roleManager.FindByNameAsync("Admin").Result;
            foreach (var claim in defaultClaims)
            {
                if (!roleManager.GetClaimsAsync(adminRole).Result.Any(c => c.Type == claim.Type && c.Value == claim.Value))
                {
                    var result = roleManager.AddClaimAsync(adminRole, claim).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Erro ao adicionar claim '{claim.Type}' à role 'Admin': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // Criar um usuário Admin padrão
            var adminEmail = "admin@domain.com";
            var adminUser = userManager.FindByEmailAsync(adminEmail).Result;
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "5521965409847", // Adicione o número de telefone aqui
                };

                var result = userManager.CreateAsync(adminUser, "Admin@123").Result;
                if (!result.Succeeded)
                {
                    throw new Exception($"Erro ao criar o usuário Admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                // Associar o usuário à role "Admin"
                var roleResult = userManager.AddToRoleAsync(adminUser, "Admin").Result;
                if (!roleResult.Succeeded)
                {
                    throw new Exception($"Erro ao associar o usuário Admin à role 'Admin': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }


                var claimsResult = await userManager.AddClaimsAsync(adminUser, defaultClaims);
                if (!claimsResult.Succeeded)
                {
                    throw new DomainException("Falha ao adicionar claims ao usuário");
                }

                // Recuperar o hash da senha gerado pelo Identity
                var passwordHash = userManager.PasswordHasher.HashPassword(adminUser, "Admin@123");

                // Criação do usuário na aplicação
                var appUser = new User
                {
                    Id = Guid.Parse(await userManager.GetUserIdAsync(adminUser)),
                    Email = adminUser.Email,
                    Password = passwordHash, // Armazena o hash da senha no banco de dados da aplicação
                    Username = adminUser.Email,
                    Phone = "5521965409847",
                    Role = UserRole.Admin, // Ajuste conforme necessário
                    Status = UserStatus.Active
                };

                var existingUser = await userRepo.GetByEmailAsync(adminUser.Email);
                if (existingUser != null)
                    throw new InvalidOperationException($"User with email {adminUser.Email} already exists");

                var createdUser = await userRepo.CreateAsync(appUser);

                if (createdUser == null)
                {
                    throw new DomainException("Falha ao registrar o usuário na aplicação");
                }

            }

        }

    }
}
