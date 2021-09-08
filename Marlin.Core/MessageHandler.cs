using Marlin.Core.Common;
using Marlin.Core.Entities;
using Marlin.Core.Interfaces;

namespace Marlin.Core
{
    public class MessageHandler : IMessageHandler
    {
        /// <summary>
        /// Sends an sms with the Helper.SendSms method
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public void Send(User user, string message)
        {
            Helper.SendSms(message, user.Properties["Mobile"]);
        }
    }
}
