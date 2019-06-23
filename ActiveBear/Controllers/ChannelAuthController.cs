using System;
using System.Linq;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActiveBear.Controllers
{
    public class ChannelAuthController : Controller
    {
        private readonly ActiveBearContext _context;

        public ChannelAuthController(ActiveBearContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AuthUserToChannel(Guid? id)
        {
            if (id == null)
                return NotFound();

            var channel = _context.Channels.Where(c => c.Id == id).FirstOrDefault();
            var currentUser = CookieService.CurrentUser(Request);
            if (channel == null || currentUser == null)
                return NotFound();

            if (ChannelAuthService.UserIsAuthed(channel, currentUser))
            {
                return Redirect(Constants.Routes.EngageChannel + "/" + channel.Id.ToString());
            }

            // User is not already authed, they need to enter the password
            ViewBag.Channel = channel;
            return View();
        }

        [HttpPost]
        public IActionResult AuthUserToChannel(Guid? id, ChannelAuth channelAuth)
        {
            if (id == null)
                return NotFound();

            var channel = _context.Channels.Where(c => c.Id == id).FirstOrDefault();
            var currentUser = CookieService.CurrentUser(Request);
            if (channel == null || currentUser == null)
                return NotFound();

            if (channelAuth.HashedKey == channel.KeyHash)
            {
                ChannelAuthService.CreateAuth(channel, currentUser);
                return Redirect(Constants.Routes.EngageChannel + "/" + channel.Id);
            }

            // Auth failed
            // TODO return this as error message
            ViewBag.Channel = channel;
            return View();
        }
    }
}
