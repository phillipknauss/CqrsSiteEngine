using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace UserInterface.Controllers
{
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
        public ActionResult Add(Commands.CreateUserCommand command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            service.CreateUser(command);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(string userId="")
        {
            ViewBag.EditableProperties = new List<ReadModel.UserProperty>()
            {
                new ReadModel.UserProperty("FirstName", null, "text", "text"),
                new ReadModel.UserProperty("LastName", null, "text", "text"),
                new ReadModel.UserProperty("Email", null, "text", "email"),
                new ReadModel.UserProperty("IsAdmin", null, "checkbox", null)
            };

            var idGuid = Guid.Empty;

            if (!Guid.TryParse(userId, out idGuid))
            {
                RedirectToAction("Index");
            }

            var service = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var query = service.GetUsers().SingleOrDefault(n => n.Id == idGuid);

            return View(query);
        }

        [HttpPost]
        public ActionResult Edit(ReadModel.UserIndexItem item)
        {
            foreach (var property in item.Properties)
            {
                SetProperty(new Commands.SetUserPropertyCommand
                                {
                    UserID = item.Id,
                    Name = property.Key,
                    Value = property.Value.Value
                });
            }

            return RedirectToAction("Details", new { id=item.Id });
        }

        static void SetProperty(Commands.SetUserPropertyCommand command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
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
