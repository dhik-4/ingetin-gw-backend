using IngetinGwAPI.Interfaces;
using IngetinGwAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace IngetinGwAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        AppDbContext _context;

        public UserRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<User> GetUserById(int Id)
        {
            User? _result = null;
            var getUser = await _context.Users.Where(u => u.Id == Id).FirstOrDefaultAsync();
            if (getUser is not null)
            {
                _result = getUser;
            }

            return _result;
        }

        public async Task<User> ValidateUsers(string Email, string Password, CancellationToken cancellationToken)
        {
            User? _result = null;
            var getUser = await _context.Users.Where(u => u.Email == Email).FirstOrDefaultAsync(cancellationToken);
            string passwMd5 = CreateMD5($"{getUser.Id}{Email}{Password}");
            if (getUser is not null && getUser.Password == passwMd5)
            {
                _result = getUser;
            }

            return _result;
        }

        public static string CreateMD5(string input)
        {
            // Use MD5.Create() to get an instance of the algorithm
            using (MD5 md5 = MD5.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Compute the hash
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to a hexadecimal string
                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
