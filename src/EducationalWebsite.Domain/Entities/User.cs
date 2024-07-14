using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EducationalWebsite.Domain.Common;
using EducationalWebsite.Domain.ValueObjects;

namespace EducationalWebsite.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(15, ErrorMessage = "Name cannot exceed 15 characters.")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(15, ErrorMessage = "LastName cannot exceed 15 characters.")]
        public string LastName { get; set; } = default!;
        
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public Email Email { get; set; } = default!;
        
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
        public SecureString Password 
        { 
            get => _password; 
            set => _password = value ?? throw new ArgumentNullException(nameof(value), "Password cannot be null."); 
        }
        
        [DataType(DataType.Date)]
        [CustomValidation(typeof(User), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender UserGender { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public Address Address { get; set; } = default!;
        
        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^\+?[0-9\s\-()]*$", ErrorMessage = "Phone number must contain digits and can include spaces, dashes, or parentheses.")]
        public string PhoneNumber { get; set; } = default!;
        
        [Required(ErrorMessage = "Role is required.")]
        public UserRole Role { get; set; }
        
        public bool IsDeleted { get; set; }

        public static ValidationResult? ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            if (dateOfBirth > DateTime.Now)
            {
                return new ValidationResult("Date of Birth cannot be in the future.");
            }
            return ValidationResult.Success;
        }

        public enum Gender
        {
            Male,
            Female
        }

        public enum UserRole
        {
            Admin,
            Customer
        }

        private SecureString? _password;


    }
}