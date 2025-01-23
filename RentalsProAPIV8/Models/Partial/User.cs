using RentalsProAPIV8.Infrastructure;

namespace RentalsProAPIV8.Models
{
    public partial class User
    {
        public string FullName => $"{FirstName} {LastName}";

        public void SetPassword(IConfiguration configuration, string password)
        {
            var hashIterations = configuration.GetValue<int>("AppSettings:HashIterations");
            var saltSize = configuration.GetValue<int>("AppSettings:SaltSize");

            PasswordSalt = Hashing.GenerateSalt(hashIterations, saltSize);
            PasswordHash = Hashing.ComputeHash(password, PasswordSalt, HashAlgorithmType.PBKDF2);
        }
    }
}
