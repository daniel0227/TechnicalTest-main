using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TechnicalTest.Common.Enums;

namespace TechnicalTest.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(50, ErrorMessage = "El nombre no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string FullName { get; set; }

        public UserType UserType { get; set; }

    }
}
