using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Models;
public class UserRegister
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string FistName { get; set; } = null!;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string? Cpf { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public DateTime? BirthDate { get; set; }    
    
    public bool Active { get; set; }        

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmePassword { get; set; } = null!;

}