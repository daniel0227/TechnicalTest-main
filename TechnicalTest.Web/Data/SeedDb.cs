using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalTest.Common.Enums;
using TechnicalTest.Web.Data.Entities;
using TechnicalTest.Web.Helpers;

namespace TechnicalTest.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
             IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckUsers();
            await CheckRolesAsync();
            await CheckUserAsync("Daniel Cardona", "daniel.cardona305@gmail.com", "300 239 3652", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(
        string fullName,
        string email,
        string phone,
        UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FullName = fullName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }
        
        private async Task CheckUsers()
        {
            if (!_context.Usuarios.Any())
            {
                _context.Usuarios.Add(new Usuario { Usuarioo = "DanielCr", Contraseña = "123456", Estado = true, NombreUsuario = "Daniel Cardona", FechaCreacion = DateTime.Now });
                _context.Usuarios.Add(new Usuario { Usuarioo = "CamiloZp", Contraseña = "123456", Estado = false, NombreUsuario = "Camilo Zapata", FechaCreacion = DateTime.Now });
                await _context.SaveChangesAsync();
            }
        }
    }
}
