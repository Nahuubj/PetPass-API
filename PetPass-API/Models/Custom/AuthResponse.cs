﻿namespace PetPass_API.Models.Custom
{
    public class AuthResponse
    {
        public int userID {  get; set; }
        public string Token { get; set; }
        public bool FirstLogin { get; set; }
        public char Role { get; set; }
        public string? Photo { get; set; }
    }
}
