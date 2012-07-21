using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Commands;

namespace UserInterface.Models
{
    public class Authentication
    {
        public static bool UserValidated(LogOnModel model)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var user = readModel.GetUsers().Where(n => n.Username == model.UserName).SingleOrDefault();
            if (user == null)
            {
                return false;
            }

            service.ValidateUser(new ValidateUserCommand()
            {
                UserID = user.Id,
                Username = model.UserName,
                Password = model.Password
            });

            bool validated = readModel.UserValidated(user.Id);

            return validated;
        }

        public static void SignOff()
        {
            FormsAuthentication.SignOut();

            var service = new Commanding.SimpleTwitterCommandServiceClient();
            service.InvalidateUser(new InvalidateUserCommand()
            {
                UserID = (Guid)HttpContext.Current.Session["UserID"]
            });
        }

        public static void SetLogin(LogOnModel model, ReadModel.UserIndexItem user)
        {
            HttpContext.Current.Session.Add("UserID", user.Id);
            var ticket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(60),
                model.RememberMe,
                user.SerializeRoles(),
                FormsAuthentication.FormsCookiePath);
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName,
                hash);
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    public class Authorization
    {
        public static void RequireAdminOrMatch(Guid id, Guid curId)
        {
            if (curId == null || id == null)
            {
                NotAuthorized();
            }
            var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var user = readModel.GetUsers().Where(n => n.Id == id).SingleOrDefault();
            if (user == null)
            {
                NotAuthorized();
            }

            if (!(user.IsAdmin || id == curId))
            {
                NotAuthorized();
            }
        }

        public static void RequireAdmin(Guid curId)
        {
            var id = curId;
            if (id == null)
            {
                NotAuthorized();
            }
            var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var user = readModel.GetUsers().Where(n => n.Id == id).SingleOrDefault();
            if (user == null)
            {
                NotAuthorized();
            }

            if (!user.IsAdmin)
            {
                NotAuthorized();
            }

        }

        static void NotAuthorized()
        {
            HttpContext.Current.Response.RedirectToRoute("Home");
        }
    }
}