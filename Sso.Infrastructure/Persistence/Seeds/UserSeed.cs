using Sso.Application.Persistence;
using Sso.Domain.Entities;
using Sso.Domain.Repositories;
using Sso.Domain.Services;
using Sso.Domain.ValueObjects;

namespace Sso.Infrastructure.Persistence.Seeds
{
    public class UserSeed(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        public async Task SeedAsync()
        {
            var email = new Email("admin@gmail.com");

            if (await userRepository.GetByEmailAsync(email) is not null)
            {
                return;
            }

            var user = new User{
                Email = email,
                PasswordHash = passwordHasher.HashPassword("admin123"),
                Name = "usuario admin"};

            await userRepository.AddAsyc(user);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
