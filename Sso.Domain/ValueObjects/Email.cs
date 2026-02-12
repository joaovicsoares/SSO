using Sso.Domain.Exceptions;

namespace Sso.Domain.ValueObjects
{
    public readonly struct Email : IEquatable<Email>
    {
        public string Value { get; }

        public Email(string value)
        {
            value = value.Trim();

            if (!IsValid(value))
                throw new SsoInvalidEmailException(value);

            Value = value;
        }

        public static bool IsValid(string emailString)
        {
            int atIndex = emailString.IndexOf('@');

            if (atIndex <= 0 || atIndex != emailString.LastIndexOf('@'))
                return false;

            string localPart = emailString[..atIndex];
            string domainPart = emailString[(atIndex + 1)..];

            if (localPart.Length == 0 || domainPart.Length == 0)
                return false;

            int dotIndex = domainPart.IndexOf('.');
            if (dotIndex <= 0 || dotIndex == domainPart.Length - 1)
                return false;

            if (emailString.Contains('_'))
                return false;

            foreach (char c in localPart)
            {
                if (!(char.IsLetterOrDigit(c) ||
                            c == '.' || c == '_' || c == '-' || c == '+'))
                    return false;
            }

            foreach (char c in domainPart)
            {
                if (!(char.IsLetterOrDigit(c) ||
                            c == '.' || c == '-'))
                    return false;
            }

            return true;
        }

        public static Email NewEmail(string value)
            => new(value);

        public bool Equals(Email other)
            => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object? obj)
            => obj is Email other && Equals(other);

        public override int GetHashCode()
            => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

        public static implicit operator string(Email email) => email.Value;

        public static explicit operator Email(string value) => new(value);

        public static bool operator ==(Email left, Email right)
            => left.Equals(right);

        public static bool operator !=(Email left, Email right)
            => !(left == right);
    }
}
