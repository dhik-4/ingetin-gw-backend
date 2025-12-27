using IngetinGwAPI.Models;

namespace IngetinGwAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User> ValidateUsers(string Email, string Password, CancellationToken cancellationToken);
        Task<User> GetUserById(int Id);
    }
}
