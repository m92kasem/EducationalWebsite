using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EducationalWebsite.Domain.ValueObjects
{
    public class Email : IEquatable<Email>
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Email cannot be empty.", nameof(value));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("Email cannot exceed 50 characters.", nameof(value));
            }

            if (!IsValidEmail(value))
            {
                throw new ArgumentException("Invalid email address format.", nameof(value));
            }

            Value = value;
        }

        public static Email Create(string email)
        {
            return new Email(email);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Email email && Equals(email);
        }

        public bool Equals(Email? other)
        {
            return other != null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator ==(Email left, Email right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(Email left, Email right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}