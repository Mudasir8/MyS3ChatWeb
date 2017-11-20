using MyS3Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyS3Chat.Library.Services
{
    public interface IFriendService
    {

        List<UserViewModel> GetUserFriendsList(string userName);
        
        List<UserViewModel> GetFriendsFriendsList(int friendId);


        string AreFriends(int userID, int visitingProfileID);

        bool RemoveFriend(int userID, int friendId);

        void SendFriendRequest(string friendUserName);

        

        List<UserViewModel> GetAllFriendRequests(string userName);

        void AcceptFriendRequest(int friendId);

        void DeclineFriendRequest(int friendId);
    }
}
