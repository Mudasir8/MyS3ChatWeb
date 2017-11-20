using MyS3Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyS3Chat.Library.Services
{
    public interface IAccountService
    {

        void RegisterNewUser(RegisterViewModel registerViewModel);

        string Login(LoginViewModel loginViewModel);

        string EmailCheck(string email);

        string UserNameCheck(string username);

        UserViewModel GetUserWithUserName(string userName);

        UserViewModel GetUserWithEmail(string email);

        UserViewModel GetUserWithID(int ID);


        List<UserViewModel> GetUsersListForLiveSearch(string searchText);


        bool EditAccount(ProfileEditViewModel vm);

        string UpdateImage(int id);

        void SaveToken(string email, string token);

        void RemoveToken(string email, string token);

        TokenViewModel GetToken(string token);


    }
}