using PetPass_API.Models.Custom;

namespace PetPass_API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> TokenReturnLogin(string username, string password);
    }
}
