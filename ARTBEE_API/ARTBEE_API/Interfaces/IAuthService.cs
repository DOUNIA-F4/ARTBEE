using ARTBEE_API.Models;

namespace ARTBEE_API.Interfaces
{
    public interface IAuthService
    {
        Task<Auth> SignUp(SignUp signUp, IFormFile image, string role);
        Task<Auth> Login(Login login);
        Task<string> UserToRoleAssign(UserToRole userRole);
    }
}
