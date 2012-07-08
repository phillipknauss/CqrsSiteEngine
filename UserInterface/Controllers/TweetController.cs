using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commands;
using UserInterface.Commanding;

namespace UserInterface.Controllers
{
    public class TweetController : Controller
    {
        //
        // GET: /Tweet/

        public ActionResult Index(string channelname="")
        {
            var service = new ReadModelService.SimpleTwitterReadModelServiceClient();
            var query = service.GetTweets();

            var channels = new List<SelectListItem>();

            foreach (var channel in service.GetChannels())
            {
                channels.Add(new SelectListItem()
                {
                    Text = channel.Name,
                    Value = channel.Id.ToString()
                });
            }
            ViewBag.Channels = channels;

            ViewBag.GetChannelName = new Func<string, string>(n =>
            {
                var channel = channels.Where(m => m.Value == n).SingleOrDefault();
                if (channel == null || channel.Value == Guid.Empty.ToString())
                {
                    return "Default";
                }

                return channel.Text;
            });

            ViewBag.GetChannelIdByName = new Func<string, Guid>(n =>
            {
                var channel = channels.Where(m => m.Text == n).SingleOrDefault();
                if (channel == null || channel.Value == Guid.Empty.ToString())
                {
                    return Guid.Empty;
                }

                return Guid.Parse(channel.Value);
            });

             if (channelname != null && channelname.Length > 0)
            {
                query = query.Where(n => n.Channel == ViewBag.GetChannelIdByName(channelname)).ToArray(); 
            }

            return View(query);
        }

        public ActionResult Add()
        {
            ViewBag.Channels = new List<SelectListItem>();

            var service = new ReadModelService.SimpleTwitterReadModelServiceClient();
            foreach (var channel in service.GetChannels())
            {
                ViewBag.Channels.Add(new SelectListItem()
                {
                    Text = channel.Name,
                    Value = channel.Id.ToString()
                });
            }

            return View();
        }

        [HttpPost]
        public ActionResult Add(PostNewTweetCommand command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            service.Execute(command);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(DeleteTweetCommand command)
        {
            var service = new Commanding.SimpleTwitterCommandServiceClient();
            service.Delete(command);

            return RedirectToAction("Index");
        }
    }
}
