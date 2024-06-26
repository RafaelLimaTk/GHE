﻿using GHE.Domain.Entities;
using GHE.Domain.Interfaces.Base;

namespace GHE.Domain.Interfaces;

public interface ITrainingRepository : IRepository<Training>
{
    Task<IEnumerable<Training>> GetByGheIdAsync(Guid gheId);
}
