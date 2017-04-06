using System.Threading.Tasks;

namespace Alice.Services
{
    /// <summary>
    /// The auth token provider.
    /// </summary>
    public interface IFirebaseAuth
    {
        void Logout();
        Task<string> LoginAsync(string email, string pwd);
        Task<bool> CreateUserAsync(string email, string pwd);
    }
}
