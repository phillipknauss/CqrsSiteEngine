using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace UserInterface.Controllers
{
    [Authorize(Roles="Admin")]
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index(string id)
        {
            var service = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var query = service.GetUsers();

            if (id != null)
            {
                query = query.Where(n => n.Id.ToString() == id).ToArray();
            }

            return View(query);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            var service = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var query = service.GetUsers();

            if (id == null)
            {
                RedirectToAction("Index");
            }

            query = query.Where(n => n.Id.ToString() == id).ToArray();

            return View(query.Single());
        }

        [HttpPost]
        public ActionResult Add(Models.UserModel item)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            service.CreateUser(new Commands.CreateUserCommand()
                {
                    Username = item.Username
                });

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string userId="")
        {
            var idGuid = Guid.Empty;

            if (!Guid.TryParse(userId, out idGuid))
            {
                RedirectToAction("Index");
            }

            var service = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var query = service.GetUsers().SingleOrDefault(n => n.Id == idGuid);

            return View(Models.UserModel.FromReadModel(query));
        }

        [HttpPost]
        public ActionResult Edit(Models.UserModel item)
        {
            var properties = new List<string>()
            {
                "Username",
                "FirstName",
                "LastName",
                "Email",
                "IsAdmin"
            };

            foreach (var property in properties)
            {
                var propInfo = item.GetType().GetProperty(property);
                if (propInfo == null)
                {
                    continue;
                }
                
                var val = propInfo.GetValue(item, null);
                if (val == null)
                {
                    continue;
                }

                SetProperty(new Commands.SetUserPropertyCommand
                {
                    UserID = item.Id,
                    Name = property,
                    Value = val.ToString()
                });
            }

            return RedirectToAction("Details", new { id=item.Id });
        }

        static void SetProperty(Commands.SetUserPropertyCommand command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            if (command.Name == "IsAdmin")
            {
                if (command.Value == true.ToString())
                {
                    service.AddUserToRole(new Commands.AddUserToRoleCommand()
                        {
                            UserID = command.UserID,
                            Role = "Admin"
                        });
                }
                else
                {
                    service.RemoveUserFromRole(new Commands.RemoveUserFromRoleCommand()
                        {
                            UserID = command.UserID,
                            Role = "Admin"
                        });
                }
            }
            
            service.SetUserProperty(command);
        }

        public ActionResult Delete(Commands.DeleteUserCommand command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            service.DeleteUser(command);

            return RedirectToAction("Index");
        }

        public ActionResult SetPassword(Guid id)
        {
            var model = new Models.ChangePasswordModel();
            // todo: Verify that user has setpassword permission
            model.UserID = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult SetPassword(Models.ChangePasswordModel command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            service.SetUserPassword(new Commands.SetUserPasswordCommand()
                {
                    UserID = command.UserID,
                    Password = command.NewPassword
                });

            return RedirectToAction("Details", new { id = command.UserID });
        }

        [HttpPost]
        public ActionResult Validate(string username, string submittedPassword)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            var readModel = new ReadModelService.SimpleTwitterReadModelServiceClient();
            
            var user = readModel.GetUsers().Where(n => n.Username == username).SingleOrDefault();

            if (user == null)
            {
                return View(false);
            }

            service.ValidateUser(new Commands.ValidateUserCommand()
            {
                UserID = user.Id,
                Username = username,
                Password = submittedPassword 
            });

            bool result = readModel.UserValidated(user.Id);

            return View(result);
        }

        
    }

}
