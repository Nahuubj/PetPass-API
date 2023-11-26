using static QRCoder.PayloadGenerator;
using System.Net;
using System.Drawing;

namespace PetPass_API.Models
{
    public class PersonCreated : Person
    {
        public int UserID { get; set; }
        public string? Image { get; set; }

        public PersonCreated(int personId, string name, string firstName, string lastName, string ci, string gender, string address, int phone, string email, short state, int userID, string image) : base(personId, name, firstName, lastName, ci, gender, address, phone, email, state)
        {
            PersonId = personId;
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            Ci = ci;
            Gender = gender;
            Address = address;
            Phone = phone;
            Email = email;
            State = state;
            UserID = userID;
            Image = image;
        }
    }
}
