using Microsoft.AspNetCore.Mvc;
using PetPass_API.Data;

namespace PetPass_API.Controllers
{
    public class UsersController : ControllerBase
    {

        private readonly DbpetPassContext _context;
        public UsersController(DbpetPassContext context)
        {
            _context = context;
        }

        //login(manejo de sesiones), cambiar contraseña, primer inicio sesion

    }
}
