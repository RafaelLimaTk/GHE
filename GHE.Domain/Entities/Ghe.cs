using GHE.Domain.Entities.Base;
using System.Diagnostics;

namespace GHE.Domain.Entities;

public class Ghe : EntityBase
{
    public string? Matricule { get; set; }
    public string? Name { get; set; }
    public string? GHE { get; set; }
    public string? Description { get; set; }
    public bool Unhealthiness { get; set; }
    public bool Dangerousness { get; set; }
    public bool NotApplicable { get; set; }

    public ICollection<Training> Trainings { get; set; }
}
