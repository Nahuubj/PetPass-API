namespace PetPass_API.Models.Custom
{
    public class AuthRecoveryPassword
    {
        public int UserID { get; set; }
        public string CodeRecovery {  get; set; }
        public string newPassword { get; set; }
    }
}
