using MyS3Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyS3Chat.Library.Services
{
    public interface IMessageService
    {
        void SendMessage(string friendUserName, string message);

        void RemoveConservation(string friendUserName);

        List<MessageViewModel> GetMessagesList(string userName);

        List<MessageViewModel> GetUnreadMessages(string friendUserName); // returns message list between user and friend whoom he chating

        List<MessageViewModel> GetMessagesForChat(string friendUserName);

        void MakeMessagesAsRead(string friendUserName);

    }
}
