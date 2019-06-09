using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActiveBear.Controllers
{
    public class ChannelController : Controller
    {
        private readonly ActiveBearContext _context;

        public ChannelController(ActiveBearContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void CreateChannel(string title, string key)
        {
            var currentUser = CookieService.CurrentUser(_context, Request);
            var channel = ChannelService.CreateChannel(title, key, _context, currentUser);
        }

        public IActionResult Engage(Guid? id)
        {
            Channel activeChannel;

            if (id == null)
                return NotFound();

            var channel = _context.Channels.Where(x => x.Id == id).FirstOrDefault();
            if (channel == null)
                return NotFound();

            // Check the current user is authorised
            var currentUser = CookieService.CurrentUser(_context, Request);
            if (currentUser == null)
                return Redirect(Constants.Routes.Login);

            if (!ChannelAuthService.AuthedUsersFor(channel, _context).Contains(currentUser))
                return Redirect(Constants.Routes.Home);

            // Gather relevant messages
            var channelMessages = ChannelService.MessagesFor(channel, _context);

            // Push our information to the view
            ViewBag.Channel = channel;
            ViewBag.Messages = channelMessages;
            ViewBag.UserNames = MessageService.LinkMessagesToUsers(channelMessages, _context);

            return View();
        }
    }
}
