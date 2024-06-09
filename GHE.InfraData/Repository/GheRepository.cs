using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.InfraData.Data;
using GHE.InfraData.Repository.Base;

namespace GHE.InfraData.Repository;

public class GheRepository : BaseRepository<Ghe>, IGheRepository
{
    public GheRepository(GheContext context) : base(context)
    {
    }
}
