using System.Threading.Tasks;

namespace Alice.Services
{
    /// <summary>
    /// The auth token provider.
    /// </summary>
    public interface IFirebaseAuth
    {
        Task<bool> LoginAsync(string email, string pwd);
        Task<bool> CreateUserAsync(string email, string pwd);
    }
}
