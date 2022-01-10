using MonsterTradingCardsServer.Models;
using System.Collections.Generic;

namespace MonsterTradingCardsServer
{
    public interface IMessageManager
    {
        Message AddMessage(User user, string content);
        IEnumerable<Message> ListMessages(User user);
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
        void RemoveMessage(User user, int messageId);
        Message ShowMessage(User user, int messageId);
        void UpdateMessage(User user, int messageId, string content);
    }
}