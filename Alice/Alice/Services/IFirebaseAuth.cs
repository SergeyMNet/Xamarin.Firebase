using System.Threading.Tasks;
using Alice.Models;

namespace Alice.Services
{
    public interface IFirebaseAuth
    {
        UserModel GetUser();

        Task<string> LoginFacebookAsync(string token);

        Task<string> LoginAsync(string email, string pwd);
        Task<bool> CreateUserAsync(string email, string pwd);
        
        void Logout();
    }
}
