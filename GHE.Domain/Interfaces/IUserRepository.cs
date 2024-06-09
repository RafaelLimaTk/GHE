using GHE.Domain.Entities;
using GHE.Domain.Interfaces.Base;

namespace GHE.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User> Login(string email, string password);
}
