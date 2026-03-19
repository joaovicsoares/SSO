namespace Sso.Domain.Exceptions
{
    public abstract class SsoException : Exception
    {
        public SsoException() : base() { }

        public SsoException(string? message) : base(message) { }

        public SsoException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}
