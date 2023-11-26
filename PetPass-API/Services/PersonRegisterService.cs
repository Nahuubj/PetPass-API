using Microsoft.EntityFrameworkCore;
using PetPass_API.Data;
using PetPass_API.Models;

namespace PetPass_API.Services
{
    public class PersonRegisterService
    {
        private readonly DbPetPassContext _context;

        public PersonRegisterService(DbPetPassContext context)
        {
            _context = context;
        }

        public async void RegisterPersonRegister(int personId, int userId)
        {
            try
            {
                PersonRegister personRegister = new PersonRegister
                {
                    PersonId = personId,
                    UserPersonId = userId
                };

                await _context.PersonRegisters.AddAsync(personRegister);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return;
            }
        }
    }
}
