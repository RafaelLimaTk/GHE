﻿using GHE.Domain.Entities;
using GHE.Domain.Interfaces.Base;

namespace GHE.Domain.Interfaces;

public interface IGheRepository : IRepository<Ghe>
{
    Task<Ghe> GetByMatriculaOrNomeAsync(string searchTerm);
}
