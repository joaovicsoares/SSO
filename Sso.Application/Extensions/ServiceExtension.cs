using Microsoft.Extensions.DependencyInjection;
using Sso.Application.Authentication;

namespace Sso.Application.Extensions
{
    public static class ServiceExtension
    {
        extension(IServiceCollection serviceCollection)
        {
            public IServiceCollection AddApplication()
            {
                serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();

                return serviceCollection;
            }
        }
    }
}
