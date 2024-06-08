namespace GHE.Models.Base;

public abstract class EntityBase
{
    public Guid Id { get; private set; }

    protected EntityBase()
    {
        this.Id = this.Id == Guid.Empty ? Guid.NewGuid() : this.Id;
    }
}
