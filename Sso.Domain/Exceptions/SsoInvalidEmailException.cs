namespace Sso.Domain.Exceptions
{
    public class SsoInvalidEmailException(string emailString) : SsoException
    {
        public string EmailString { get; } = emailString;
    }
}
