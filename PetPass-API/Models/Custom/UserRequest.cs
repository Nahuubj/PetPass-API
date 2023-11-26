using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PetPass_API.Models.Custom
{
    public class UserRequest
    {

        public string Username { get; set; } = null!;

        public string Userpassword { get; set; } = null!;
    }
}
