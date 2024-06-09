using GHE.Domain.Entities.Base;

namespace GHE.Domain.Entities;

public class Training : EntityBase
{
    public string? TrainingName { get; set; }
    public DateTime TrainingDate { get; set; }
    public DateTime? TrainingDateFinal { get; set; }
    public string? ASO { get; set; }
    public Guid GheId { get; set; }

    public Ghe? Ghe { get; set; }

    protected Training() { }

    public Training(string trainingName, DateTime trainingDate, DateTime? trainingDateFinal, string aso)
    {
        TrainingName = trainingName;
        TrainingDate = trainingDate;
        TrainingDateFinal = trainingDateFinal;
        ASO = aso;
    }
}
