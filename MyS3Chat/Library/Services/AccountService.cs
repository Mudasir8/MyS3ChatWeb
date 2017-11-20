using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyS3Chat.Models;
using MyS3Chat.Library.Entities;
using System.Web.Security;
using MyS3Chat.Library.DAL;
using System.IO;

namespace MyS3Chat.Library.Services
{
    public class AccountService : IAccountService
    {
        DataContext db = new DataContext();

        public TokenViewModel GetToken(string token)
        {
            try
            {


                Token tkn = db.Tokens.Where(x => x.TokenId.Equals(token)).FirstOrDefault();

                TokenViewModel tVM = new TokenViewModel()
                {
                    ID = tkn.ID,
                    Email = tkn.Email,
                    TokenId = tkn.TokenId
                };
                return tVM;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public bool EditAccount(ProfileEditViewModel vm)
        {
            try
            {
                User user = db.Users.Find(vm.ID);

                user.DateUpdated = DateTime.Now;
                user.FirstName = vm.FirstName;
                user.LastName = vm.LastName;
               
                user.Status = vm.Status;
                user.StatusUpdatedDate = DateTime.Now;

                db.SaveChanges();

                return true;
            }
         catch(Exception e)
            {
                return false;
            }

        }

        public string EmailCheck(string email)
        {
            User user = db.Users.Where(x => x.Email.Equals(email)).FirstOrDefault();
            if (user == null)
            {
                return "ok";
            }
            else
            {
                return "error";
            }
        }

        public List<UserViewModel> GetUsersListForLiveSearch(string searchText)
        {
            List<User> list = db.Users.Where(x => x.FirstName.Contains(searchText) || 
                                                  x.LastName.Contains(searchText) ||
                                                  x.UserName.Contains(searchText) ||
                                                  x.Email.Contains(searchText)
                                            ).ToList();


            List<UserViewModel> viewList = new List<UserViewModel>();
            foreach (var item in list)
            {
                UserViewModel a = new UserViewModel()
                {
                    UserName = item.UserName,
                    ImagePath = item.ImagePath,
                    ID = item.ID,
                    FirstName = item.FirstName,
                    LastName = item.LastName

                };
                viewList.Add(a);
            }

            return viewList;
        }

        public UserViewModel GetUserWithEmail(string email)
        {
            try
            {
                var user = db.Users.Where(x => x.Email.Equals(email)).FirstOrDefault();

                UserViewModel viewmodel = new UserViewModel()
                {
                    DateUpdated = user.DateUpdated,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    ImagePath = user.ImagePath,
                    LastName = user.LastName,
                    Password = user.Password,
                    Status = user.Status,
                    ID = user.ID,
                    UserName = user.UserName,
                    VisibleTo = user.VisibleTo,
                    DateRegistered = user.DateRegistered,
                    EmailConfirmed = user.EmailConfirmed,
                    StatusUpdatedDate = user.StatusUpdatedDate

                };

                return viewmodel;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public UserViewModel GetUserWithID(int ID)
        {
            throw new NotImplementedException();
        }

      

        public UserViewModel GetUserWithUserName(string userName)
        {
            try
            {

                var user = db.Users.Where(x => x.UserName.Equals(userName)).FirstOrDefault();

                UserViewModel viewmodel = new UserViewModel()
                {
                    DateUpdated = user.DateUpdated,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    ImagePath = user.ImagePath,
                    LastName = user.LastName,
                    Password = user.Password,
                    Status = user.Status,
                    ID = user.ID,
                    UserName = user.UserName,
                    VisibleTo = user.VisibleTo,
                    DateRegistered = user.DateRegistered,
                    EmailConfirmed = user.EmailConfirmed,
                    StatusUpdatedDate = user.StatusUpdatedDate

                };

                return viewmodel;
            }
            catch(Exception e)
            {
                 HttpContext.Current.Response.Redirect("~/");
                return null;
            }
        }

        public string Login(LoginViewModel loginViewModel)
        {
            // Get user
            User user = db.Users.Where(x => x.Email.Equals(loginViewModel.Email) && x.Password.Equals(loginViewModel.Password)).FirstOrDefault();

            // check if user exists
            if (user != null)
            {
               
                return user.UserName;

            }
            else
            {
                return null;
            }
        }

        public void RegisterNewUser(RegisterViewModel registerViewModel)
        {
            // make new user and transer data from viewmodel
            User user = new User()
            {

                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
                EmailConfirmed = false,
                VisibleTo = "Public",
                ImagePath = "noimage.png",
                Password = registerViewModel.Password,
                DateRegistered = DateTime.Now,
                DateUpdated = DateTime.Now,
                StatusUpdatedDate = DateTime.Now,
                Status = ""

            };

            // add to db
            db.Users.Add(user);
            db.SaveChanges();

            


            // set auth cookie for newly registered user
            FormsAuthentication.SetAuthCookie(registerViewModel.UserName, false);
        }

        public void SaveToken(string email, string token)
        {
            Token tkn = new Token()
            {
                Email = email,
                TokenId = token

            };
            db.Tokens.Add(tkn);
            db.SaveChanges();

        }

        public string UpdateImage(int id)
        {
            try
            {
                for (int i = 0; i < 1; i++)
                {
                    var file = System.Web.HttpContext.Current.Request.Files[i];


                    // upload image
                    if (file != null && file.ContentLength > 0)
                    {
                        var guid = Guid.NewGuid();
                        var fileName = Path.GetFileName(file.FileName);
                        var extension = Path.GetExtension(fileName);
                        var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/"), id + "-" + guid + extension);

                        file.SaveAs(path);

                        // update imagepath in profile with userid

                        string imagePath = id + "-" + guid + extension;

                        var user = db.Users.Where(x => x.ID.Equals(id)).FirstOrDefault();
                        user.ImagePath = imagePath;

                        //db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        

                    }
                }
                return "ok";
            }
            catch(Exception e)
            {
                return e.ToString();
            }
            
        }

        public string UserNameCheck(string username)
        {
            var user = db.Users.Where(x => x.UserName.Equals(username)).FirstOrDefault();
            if (user == null)
            {
                return "ok";
            }
            else return "error";
        }

        public void RemoveToken(string email, string token)
        {
            Token tkn = db.Tokens.Where(x => x.Email == email && x.TokenId == token).FirstOrDefault();
          
            db.Tokens.Remove(tkn);
            db.SaveChanges();
        }
    }
}