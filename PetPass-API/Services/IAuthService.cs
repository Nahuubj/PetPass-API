using PetPass_API.Models.Custom;

namespace PetPass_API.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> TokenReturnLogin(UserRequest userRequest);
    }
}
