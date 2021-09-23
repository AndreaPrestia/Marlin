using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;

namespace Marlin.Core.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        public void Send(User user, string message)
        {
            Helper.SendSms($"Your access code: {message}", user.Mobile.ToString());
        }
    }
}
