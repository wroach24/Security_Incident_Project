using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Security_Response_Program.Services;

public interface IPasswordHashingService
{
    /// <summary>
    /// Hashes a password and returns the hash and salt as a tuple.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A tuple containing the hash and salt.</returns>
    (string Hash, string Salt) HashPassword(string password);

    /// <summary>
    /// Verifies a password by comparing its hash with a stored hash.
    /// </summary>
    /// <param name="enteredPassword">The password entered by the user.</param>
    /// <param name="storedHash">The hash of the stored password.</param>
    /// <param name="storedSalt">The salt used to hash the stored password.</param>
    /// <returns>True if the entered password is correct, false otherwise.</returns>
    bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt);
}

/// <summary>
/// Provides password hashing and verification services.
/// </summary>
public class PasswordHashingService : IPasswordHashingService
{
    /// <summary>
    /// Hashes a password and returns the hash and salt as a tuple.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>A tuple containing the hash and salt.</returns>
    public (string Hash, string Salt) HashPassword(string password)
    {
        // Generate a random salt.
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password using PBKDF2 with the salt.
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Return the hash and salt as a tuple.
        return (hashed, Convert.ToBase64String(salt));
    }

    /// <summary>
    /// Verifies a password by comparing its hash with a stored hash.
    /// </summary>
    /// <param name="enteredPassword">The password entered by the user.</param>
    /// <param name="storedHash">The hash of the stored password.</param>
    /// <param name="storedSalt">The salt used to hash the stored password.</param>
    /// <returns>True if the entered password is correct, false otherwise.</returns>
    public bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        // Convert the stored salt from base64 to a byte array.
        byte[] salt = Convert.FromBase64String(storedSalt);

        // Hash the entered password using PBKDF2 with the stored salt.
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: enteredPassword,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        // Compare the resulting hash with the stored hash.
        return hashed == storedHash;
    }
}
