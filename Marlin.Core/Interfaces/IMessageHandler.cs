using Marlin.Core.Entities;

namespace Marlin.Core.Interfaces
{
    public interface IMessageHandler
    {
        public void Send(User user, string message);
    }
}
