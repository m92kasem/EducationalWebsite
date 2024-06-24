using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Common;

namespace EducationalWebsite.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(40, ErrorMessage = "Address cannot exceed 40 characters.")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain digits only.")]
        public string PhoneNumber { get; set; }
        
        [Required]
        public bool IsAdmin { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; }

    }
}