using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.InfraData.Data;
using GHE.InfraData.Repository.Base;

namespace GHE.InfraData.Repository;

public class TrainingRepository : BaseRepository<Training>, ITrainingRepository
{
    public TrainingRepository(GheContext context) : base(context)
    {
    }
}
