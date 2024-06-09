using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.InfraData.Data;
using GHE.InfraData.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GHE.InfraData.Repository;

public class TrainingRepository : BaseRepository<Training>, ITrainingRepository
{
    public TrainingRepository(GheContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Training>> GetByGheIdAsync(Guid gheId)
    {
        return await Entities.Where(t => t.GheId == gheId).ToListAsync();
    }
}
