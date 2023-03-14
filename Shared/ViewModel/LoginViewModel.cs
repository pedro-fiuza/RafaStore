using System.ComponentModel.DataAnnotations;

namespace RafaStore.Shared
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório."), EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Senha é obrigatório.")]
        public string Password { get; set; } = string.Empty;
    }
}
