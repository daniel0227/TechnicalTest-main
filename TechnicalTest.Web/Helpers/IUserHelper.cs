using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TechnicalTest.Web.Data.Entities;

namespace TechnicalTest.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<SignInResult> ValidatePasswordAsync(User user, string password);
    }
}
