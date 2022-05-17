using Marlin.Core.Entities;

namespace Marlin.Core.Interfaces
{
    public interface IEventHandler
    {
        void WriteEvent(Event eventInstance);
    }
}
