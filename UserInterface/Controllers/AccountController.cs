using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using UserInterface.Models;
using Commands;
using System.Web;

namespace UserInterface.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: /Account/LogOn
        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return ContextDependentView();
        }

        // POST: /Account/JsonLogOn
        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonLogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Authentication.UserValidated(model))
                {
                    var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
                    var user = readModel.GetUsers().Where(n => n.Username == model.UserName).SingleOrDefault();

                    Authentication.SetLogin(model, user);
                    
                    return Json(new { success = true, redirect = returnUrl });
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        // POST: /Account/LogOn
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Authentication.UserValidated(model))
                {
                    var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
                    var user = readModel.GetUsers().Where(n => n.Username == model.UserName).SingleOrDefault();

                    Authentication.SetLogin(model, user);

                    return Url.IsLocalUrl(returnUrl)
                               ? (ActionResult) Redirect(returnUrl)
                               : RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/LogOff
        public ActionResult LogOff()
        {
            Authentication.SignOff();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return ContextDependentView();
        }

        bool DoRegister(RegisterModel model)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
            service.CreateUser(new CreateUserCommand()
            {
                Username = model.UserName
            });

            var user = readModel.GetUsers().Where(n => n.Username == model.UserName).SingleOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Command failed.");
                return false;
            }

            service.SetUserPassword(new SetUserPasswordCommand()
            {
                UserID = user.Id,
                Password = model.Password
            });

            service.SetUserProperty(new SetUserPropertyCommand()
            {
                UserID = user.Id,
                Name = "Email",
                Value = model.Email
            });

            FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
            return true;
        }

        // POST: /Account/JsonRegister
        [AllowAnonymous]
        [HttpPost]
        public ActionResult JsonRegister(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (DoRegister(model))
                {
                    var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
                    var user = readModel.GetUsers().Where(n => n.Username == model.UserName).SingleOrDefault();
                    Session.Add("UserID", user.Id);
                    return Json(new { success = true });
                }
            }
            
            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        // POST: /Account/Register
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (DoRegister(model))
                {
                    var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
                    var user = readModel.GetUsers().Where(n => n.Username == model.UserName).SingleOrDefault();
                    Session.Add("UserID", user.Id);
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Account/ChangePassword
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                throw new NotImplementedException();
                
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            ViewBag.FormAction = actionName;
            return View();
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors
                .Select(error => error.ErrorMessage));
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
