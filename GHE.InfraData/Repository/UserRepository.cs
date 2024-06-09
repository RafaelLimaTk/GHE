using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.InfraData.Data;
using GHE.InfraData.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GHE.InfraData.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(GheContext context) : base(context)
    {
    }

    public async Task<User> Login(string email, string password)
    {
        return await Entities.Where(x => x.Email == email && x.Password == password).AsNoTracking().FirstOrDefaultAsync();
    }
}