﻿using System.ComponentModel.DataAnnotations;

namespace BackendPractice.AuthModle
{
    public class RegisterDBModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [Key]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public  int Age { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
