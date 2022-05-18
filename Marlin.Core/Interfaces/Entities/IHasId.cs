namespace Marlin.Core.Interfaces.Entities
{
    public interface IHasId<T>
    {
        T Id { get; set; }
    }
}
