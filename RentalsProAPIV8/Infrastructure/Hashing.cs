using System.Security.Cryptography;
using System.Text;

namespace RentalsProAPIV8.Infrastructure
{
    public enum HashAlgorithmType
    {
        SHA1,
        SHA256,
        SHA512,
        MD5,
        PBKDF2
    }

    public class Hashing
    {
        public static string ComputeHash(string plainText, string salt, HashAlgorithmType hashAlgorithm)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(salt)) throw new ArgumentNullException(nameof(salt));

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            byte[] hashBytes = hashAlgorithm switch
            {
                HashAlgorithmType.SHA1 or HashAlgorithmType.SHA256 or HashAlgorithmType.SHA512 or HashAlgorithmType.MD5
                    => ComputeStandardHash(hashAlgorithm, plainTextBytes, saltBytes),
                HashAlgorithmType.PBKDF2 => ComputePbkdf2Hash(plainText, salt),
                _ => throw new ArgumentOutOfRangeException(nameof(hashAlgorithm), "Unsupported hash algorithm.")
            };

            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
            hashBytes.CopyTo(hashWithSaltBytes, 0);
            saltBytes.CopyTo(hashWithSaltBytes, hashBytes.Length);

            return Convert.ToBase64String(hashWithSaltBytes);
        }

        public static string GenerateSalt(int iterations, int saltSize)
        {
            if (iterations < 1) throw new ArgumentOutOfRangeException(nameof(iterations), "Iterations must be greater than 0.");
            if (saltSize < 1) throw new ArgumentOutOfRangeException(nameof(saltSize), "Salt size must be greater than 0.");

            byte[] saltBytes = new byte[saltSize];
            RandomNumberGenerator.Fill(saltBytes);

            return $"{iterations}.{Convert.ToBase64String(saltBytes)}";
        }

        private static byte[] ComputeStandardHash(HashAlgorithmType algorithmType, byte[] plainTextBytes, byte[] saltBytes)
        {
            using HashAlgorithm hashAlgorithm = HashAlgorithm.Create(algorithmType.ToString()) ??
                                                throw new ArgumentOutOfRangeException(nameof(algorithmType), "Unsupported hash algorithm.");

            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
            plainTextBytes.CopyTo(plainTextWithSaltBytes, 0);
            saltBytes.CopyTo(plainTextWithSaltBytes, plainTextBytes.Length);

            return hashAlgorithm.ComputeHash(plainTextWithSaltBytes);
        }

        private static byte[] ComputePbkdf2Hash(string plainText, string salt)
        {
            var saltParts = salt.Split('.');
            if (saltParts.Length != 2 || !int.TryParse(saltParts[0], out int iterations) || iterations < 1)
                throw new FormatException("Salt must be in the format '{iterations}.{saltstring}' with a positive iteration count.");

            byte[] saltBytes = Convert.FromBase64String(saltParts[1]);

            using var pbkdf2 = new Rfc2898DeriveBytes(plainText, saltBytes, iterations, HashAlgorithmName.SHA512);
            return pbkdf2.GetBytes(64); // Length in bytes for SHA-512
        }
    }
}
