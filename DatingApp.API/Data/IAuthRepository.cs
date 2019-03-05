using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register (User user, string pass);
        Task<User> Login(string userName, string pass );
        Task<bool> userExist(string userName);

    }
}