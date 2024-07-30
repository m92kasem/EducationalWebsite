using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using EducationalWebsite.Domain.Common;
using EducationalWebsite.Domain.ValueObjects;

namespace EducationalWebsite.Domain.Entities
{
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(15, ErrorMessage = "First Name cannot exceed 15 characters.")]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(15, ErrorMessage = "Last Name cannot exceed 15 characters.")]
        public string LastName { get; set; } = default!;

        [DataType(DataType.Date)]
        [CustomValidation(typeof(ApplicationUser), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender UserGender { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public Address Address { get; set; } = default!;

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^\+?[0-9\s\-()]*$", ErrorMessage = "Phone number must contain digits and can include spaces, dashes, or parentheses.")]
        public string MobileNumber { get; set; } = default!;

        public bool IsDeleted { get; set; }
        public bool IsEmailConfirmed { get; set; }

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
    }
}