using PetPass_API.Data;
using PetPass_API.Models;

namespace PetPass_API.Services
{
    public class PetRegisterService
    {
        private readonly DbPetPassContext _context;

        public PetRegisterService(DbPetPassContext context)
        {
            _context = context;
        }

        public async void RegisterPet(int petId, int userId)
        {
            try
            {
                PetRegister petRegister = new PetRegister
                {
                    PetId = petId,
                    UserPersonId = userId
                };

                await _context.PetRegisters.AddAsync(petRegister);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }
    }
}
