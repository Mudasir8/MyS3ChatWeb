using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyS3Chat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);



            #region AjaxController
            routes.MapRoute("RemoveFriend", "Ajax/RemoveFriend", new { controller = "Ajax", action = "RemoveFriend" });
            routes.MapRoute("RemoveConservation", "Ajax/RemoveConservation", new { controller = "Ajax", action = "RemoveConservation" });

            routes.MapRoute("EmailCheck", "Ajax/EmailCheck", new { controller = "Ajax", action = "EmailCheck" });
            routes.MapRoute("UserNameCheck", "Ajax/UserNameCheck", new { controller = "Ajax", action = "UserNameCheck" });
            routes.MapRoute("LiveSearch", "Ajax/LiveSearch", new { controller = "Ajax", action = "LiveSearch" });
            routes.MapRoute("UploadImage", "Ajax/UploadImage/{id}", new { controller = "Ajax", action = "UploadImage", id = UrlParameter.Optional });
            routes.MapRoute("AddFriend", "Ajax/AddFriend", new { controller = "Ajax", action = "AddFriend" });
            routes.MapRoute("DisplayFriendRequests", "Ajax/DisplayFriendRequests", new { controller = "Ajax", action = "DisplayFriendRequests" });
            routes.MapRoute("AcceptFriendRequest", "Ajax/AcceptFriendRequest", new { controller = "Ajax", action = "AcceptFriendRequest" });
            routes.MapRoute("DeclineFriendRequest", "Ajax/DeclineFriendRequest", new { controller = "Ajax", action = "DeclineFriendRequest" });
            routes.MapRoute("SendMessage", "Ajax/SendMessage", new { controller = "Ajax", action = "SendMessage" });
            routes.MapRoute("DisplayUnreadMessages", "Ajax/DisplayUnreadMessages", new { controller = "Ajax", action = "DisplayUnreadMessages" });

            #endregion

            routes.MapRoute("ResetPassword", "Account/ResetPassword/{token}", new { controller = "Account", action = "ResetPassword" });


            routes.MapRoute("Edit", "Profile/Edit/{username}", new { controller = "Profile", action = "Edit" });

            routes.MapRoute("Logout", "Account/Logout", new { controller = "Account", action = "Logout" });
            routes.MapRoute("Login", "Account/Index", new { controller = "Account", action = "Index" });
            routes.MapRoute("Register", "Account/Register", new { controller = "Account", action = "Register" });

            routes.MapRoute("ForgotPassword", "Account/ForgotPassword", new { controller = "Account", action = "ForgotPassword" });






            routes.MapRoute("Show", "Profile/Show", new { controller = "Profile", action = "Show" });


            routes.MapRoute("Profile","Profile/{username}", new { controller = "Profile", action = "Index" });

            routes.MapRoute("Chat", "Chat/{userName}", new { controller = "Chat", action = "Index" });

            routes.MapRoute("User", "{username}", new { controller = "User", action = "Username" });

            routes.MapRoute("Defaul", "", new { controller = "User", action = "UserName" });


        }
    }
}
