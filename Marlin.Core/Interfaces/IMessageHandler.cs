using Marlin.Core.Interfaces.Entities;

namespace Marlin.Core.Interfaces
{
    public interface IMessageHandler<in T, T1> where T : IIdentity<T1>, new()
    {
        void Send(T identity, string message);
    }
}
