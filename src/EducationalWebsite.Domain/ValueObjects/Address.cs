using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EducationalWebsite.Domain.ValueObjects
{
    public class Address : IEquatable<Address>
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string ZipCode { get; }

        private Address(string street, string city, string state, string zipCode)
        {
            if (string.IsNullOrWhiteSpace(street) || street.Length > 40)
            {
                throw new ArgumentException("Street cannot be empty and must be less than 40 characters.", nameof(street));
            }

            if (string.IsNullOrWhiteSpace(city) || city.Length > 40)
            {
                throw new ArgumentException("City cannot be empty and must be less than 40 characters.", nameof(city));
            }

            if (string.IsNullOrWhiteSpace(state) || state.Length > 40)
            {
                throw new ArgumentException("State cannot be empty and must be less than 40 characters.", nameof(state));
            }

            if (!IsValidZipCode(zipCode))
            {
                throw new ArgumentException("Invalid zip code format.", nameof(zipCode));
            }

            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
        }

        public static Address Create(string street, string city, string state, string zipCode)
        {
            return new Address(street, city, state, zipCode);
        }

        public static bool IsValidZipCode(string zipCode)
        {
            var zipCodeRegex = new Regex(@"^\d{4}$", RegexOptions.Compiled);
            return zipCodeRegex.IsMatch(zipCode); 
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Address);
        }
        
        public bool Equals(Address? other)
        {
            return other is Address address &&
                   Street == address.Street &&
                   City == address.City &&
                   State == address.State &&
                   ZipCode == address.ZipCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, State, ZipCode);
        }
        public override string ToString()
        {
            return $"{Street}, {City}, {State}, {ZipCode}";
        }
    }
}