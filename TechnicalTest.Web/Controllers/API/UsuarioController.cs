using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalTest.Common.Models;
using TechnicalTest.Common.Models.Request;
using TechnicalTest.Common.Models.Response;
using TechnicalTest.Web.Data;
using TechnicalTest.Web.Data.Entities;

namespace TechnicalTest.Web.Controllers.API
{
    [ApiController]
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UsuarioController(
            DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(AddUserRequest request)
        {
            // Validaciones
            var validations = ValidateUserRequest(request);
            if (validations != null)
            {
                return BadRequest(new Response<object>
                {
                    RealizadoCorrectamente = validations.RealizadoCorrectamente,
                    Mensaje = validations.Mensaje
                });
            }

            var user = await _dataContext.Usuarios.FirstOrDefaultAsync(u => u.Usuarioo.Equals(request.Usuario));
            if (user != null)
            {
                return BadRequest(new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = "El usuario ya se encuentra registrado."
                });
            }

            // agregar usuario a la base de datos
            var newUser = new Usuario
            {
                Usuarioo = request.Usuario,
                Contraseña = request.Contrasena,
                Estado = request.Estado,
                NombreUsuario = request.NombreUsuario,
                FechaCreacion = request.FechaCreacion
            };

            _dataContext.Usuarios.Add(newUser);
            await _dataContext.SaveChangesAsync();

            return Ok(new Response<object>
            {
                RealizadoCorrectamente = true,
                Mensaje = "El usuario fue agregado correctamente."
            });
        }

        private Response<object> ValidateUserRequest(AddUserRequest request)
        {
            // usuario
            if (string.IsNullOrEmpty(request.Usuario))
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = "El campo usuario es requerida."
                };
            }

            if (!string.IsNullOrEmpty(request.Usuario) && request.Usuario.Length > 50)
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = "El campo usuario no puede tener mas de 50 caracteres."
                };
            }

            // Contraseña
            if (string.IsNullOrEmpty(request.Contrasena))
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = "La contraseña es requerida."
                };
            }

            if (!string.IsNullOrEmpty(request.Contrasena) && request.Contrasena.Length > 20)
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = "La contraseña no puede tener mas de 20 caracteres."
                };
            }

            // NombreUsuario
            if (string.IsNullOrEmpty(request.NombreUsuario))
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = "El nombre de usuario es requerido."
                };
            }

            if (!string.IsNullOrEmpty(request.NombreUsuario) && request.NombreUsuario.Length > 100)
            {
                return new Response<object>
                {
                    RealizadoCorrectamente = false,
                    Mensaje = "El nombre de usuario no puede tener mas de 100 caracteres."
                };
            }

            return null;
        }


        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _dataContext.Usuarios.ToListAsync();

            var response = new List<UserInformationResponse>(users.Select(u => new UserInformationResponse
            {
                Usuario = u.Usuarioo,
                Contraseña = u.Contraseña,
                Estado = u.Estado,
                NombreUsuario = u.NombreUsuario,
                FechaCreacion = u.FechaCreacion
            }).ToList());

            return Ok(response);
        }

        [HttpPost]
        [Route("GetUserState")]
        public async Task<IActionResult> GetUserState(UserStateRequest request)
        {
            string finalMessage = "El usuario es requerido.";
            bool consultaCompletada = false;
            UserInformationResponse result = null;

            if (!string.IsNullOrEmpty(request.Usuario))
            {
                // Consultar usuario
                var user = await _dataContext.Usuarios.FirstOrDefaultAsync(u => u.Usuarioo.Equals(request.Usuario));

                if (user != null)
                {
                    if (user.Estado)
                    {
                        consultaCompletada = true;
                        finalMessage = $"El usuario : { request.Usuario }, esta actualmente activo.";

                        // Establecer valores valores
                        result = new UserInformationResponse
                        {
                            Usuario = user.Usuarioo,
                            Contraseña = user.Contraseña,
                            Estado = user.Estado,
                            NombreUsuario = user.NombreUsuario,
                            FechaCreacion = user.FechaCreacion
                        };
                    }
                    else
                        finalMessage = $"El usuario : { request.Usuario }, Esta inactivo.";
                }
                else
                    finalMessage = $"El usuario : { request.Usuario }, no esta registrado.";
            }

            return Ok(new Response<UserInformationResponse>
            {
                RealizadoCorrectamente = consultaCompletada,
                Mensaje = finalMessage,
                Resultado = result
            });
        }
    }
}
