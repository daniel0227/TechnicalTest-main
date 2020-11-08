using System;
using System.ComponentModel.DataAnnotations;

namespace TechnicalTest.Web.Data.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        [MaxLength(50, ErrorMessage = "El usuario no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El usuario es requerido.")]
        [Display(Name = "Usuario")]
        public string Usuarioo { get; set; }
        [MaxLength(20, ErrorMessage = "La {0} no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "La contraseña es requerida.")]
        public string Contraseña { get; set; }
        [Required(ErrorMessage = "El estado es requerido.")]
        public bool Estado { get; set; }
        [MaxLength(100, ErrorMessage = "El nombre del usuario no puede tener mas de {1} caracteres.")]
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [Display(Name = "Nombre de usuario")]
        public string NombreUsuario { get; set; }
        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de creación")]
        [Required(ErrorMessage = "La fecha de creación es requerida.")]
        public DateTime FechaCreacion { get; set; }
    }
}
