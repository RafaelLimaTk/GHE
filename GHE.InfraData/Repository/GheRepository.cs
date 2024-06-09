using GHE.Domain.Entities;
using GHE.Domain.Interfaces;
using GHE.InfraData.Data;
using GHE.InfraData.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GHE.InfraData.Repository;

public class GheRepository : BaseRepository<Ghe>, IGheRepository
{
    public GheRepository(GheContext context) : base(context)
    {
    }

    public async Task<Ghe> GetByMatriculaOrNomeAsync(string searchTerm)
    {
        var lowerCaseSearchTerm = searchTerm.ToLower();
        return await Entities
            .FirstOrDefaultAsync(g => g.Matricule.ToLower() == lowerCaseSearchTerm || g.Name.ToLower() == lowerCaseSearchTerm);
    }
}
