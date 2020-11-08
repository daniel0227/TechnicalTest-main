using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalTest.Common.Models;
using TechnicalTest.Common.Models.Request;
using TechnicalTest.Common.Services;
using TechnicalTest.Web.Data;
using TechnicalTest.Web.Data.Entities;
using TechnicalTest.Web.Helpers;

namespace TechnicalTest.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;
        private readonly IConfigurationServiceHelper _configurationServiceHelper;

        public UsuariosController(
            DataContext context,
            IApiService apiService,
            IConfigurationServiceHelper configurationServiceHelper)
        {
            _context = context;
            _apiService = apiService;
            _configurationServiceHelper = configurationServiceHelper;
        }

        // Obtener la informacion de los usuarios
        public async Task<IActionResult> Index()
        {
            // Consumir Api de usuarios
            var response = await _apiService.GetListAsync<UserInformationResponse>(_configurationServiceHelper.GetUrlBaseService(), "/api", "/Usuario/GetAllUsers", "bearer", _configurationServiceHelper.GetToken());

            // Establecer informacion en la entidad
            var userList = new List<Usuario>();
            if (response != null && response.RealizadoCorrectamente)
            {
                var allUsers = (List<UserInformationResponse>)response.Resultado;
                userList.AddRange(allUsers.Select(u => new Usuario
                {
                    Usuarioo = u.Usuario,
                    Contraseña = u.Contraseña,
                    Estado = u.Estado,
                    NombreUsuario = u.NombreUsuario,
                    FechaCreacion = u.FechaCreacion
                }));
            }

            return View(userList);
        }
        
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            // Validar si el usuario esta activo
            var request = new UserStateRequest { Usuario = id };
            var response = await _apiService.PostUserState(_configurationServiceHelper.GetUrlBaseService(), "/api", "/Usuario/GetUserState", request, "bearer", _configurationServiceHelper.GetToken());
            var finalUser = new UserInformationResponse();

            if (response != null && response.RealizadoCorrectamente)
            {
                finalUser.Usuario = response.Resultado.Usuario;
                finalUser.Contraseña = response.Resultado.Contraseña;
                finalUser.Estado = response.Resultado.Estado;
                finalUser.NombreUsuario = response.Resultado.NombreUsuario;
                finalUser.FechaCreacion = response.Resultado.FechaCreacion;
            }
            else
            {
                finalUser.Estado = false;
                ViewBag.Mensaje = response.Mensaje;
            }

            return View(finalUser);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usuarioo,Contraseña,Estado,NombreUsuario,FechaCreacion")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Guardar usuario 
                var request = new AddUserRequest
                {
                    Usuario = usuario.Usuarioo,
                    Contrasena = usuario.Contraseña,
                    Estado = usuario.Estado,
                    NombreUsuario = usuario.NombreUsuario,
                    FechaCreacion = usuario.FechaCreacion
                };
                var response = await _apiService.PostAddUser(_configurationServiceHelper.GetUrlBaseService(), "/api", "/Usuario/AddUser", request, "bearer", _configurationServiceHelper.GetToken());

                if (response != null && !response.RealizadoCorrectamente)
                {
                    ModelState.AddModelError(string.Empty, response.Mensaje);
                    return View(usuario);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

    }
}
