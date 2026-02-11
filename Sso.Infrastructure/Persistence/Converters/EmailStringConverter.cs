using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sso.Domain.ValueObjects;

namespace Sso.Infrastructure.Persistence.Converters
{
    public class EmailStringConverter()
        : ValueConverter<Email, string>(x => x.Value, x => new Email(x));
}
