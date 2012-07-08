using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;

namespace UserInterface.Controllers
{
    public class ChannelController : Controller
    {
        //
        // GET: /Channel/

        public ActionResult Index()
        {
            var service = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var query = service.GetChannels();

            return View(query);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(CreateNewChannelCommand command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();

            service.CreateNewChannel(command);

            return RedirectToAction("");
        }
    }
}
