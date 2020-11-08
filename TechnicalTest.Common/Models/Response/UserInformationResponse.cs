using System;
using System.Collections.Generic;
using System.Text;

namespace TechnicalTest.Common.Models
{
    public class UserInformationResponse
    {
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public bool Estado { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string EstadoActual => Estado ? "Activo" : "Inactivo";
    }
}
