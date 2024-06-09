using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.InfraData.Data;
using GHE.InfraData.Repository.Base;

namespace GHE.InfraData.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(GheContext context) : base(context)
    {
    }
}