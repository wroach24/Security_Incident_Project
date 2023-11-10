using Security_Response_Program.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Security_Response_Program.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <returns>True if the user was successfully added to the database, otherwise false.</returns>
        Task<bool> SignUp(string username, string password);

        /// <summary>
        /// Authenticates a user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user to authenticate.</param>
        /// <param name="password">The password of the user to authenticate.</param>
        /// <returns>True if the user was successfully authenticated, otherwise false.</returns>
        Task<bool> Login(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IncidentDbContext _context;
        private readonly IPasswordHashingService _hashingService;

        public UserService(IncidentDbContext context, IPasswordHashingService hashingService)
        {
            _context = context;
            _hashingService = hashingService;
        }

        /// <summary>
        /// Creates a new user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <returns>True if the user was successfully added to the database, otherwise false.</returns>
        public async Task<bool> SignUp(string username, string password)
        {

            var (hash, salt) = _hashingService.HashPassword(password);

            var user = new User()
            {
                Username = username,
                PasswordHash = hash,
                PasswordSalt = salt,
                CreatedDate = DateTime.Now,

            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Authenticates a user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user to authenticate.</param>
        /// <param name="password">The password of the user to authenticate.</param>
        /// <returns>True if the user was successfully authenticated, otherwise false.</returns>
        public async Task<bool> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return false;
            }

            return _hashingService.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        }

    }

}
