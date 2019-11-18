using System;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> AuthUserToChannel(Guid? id)
        {
            if (id == null)
                return NotFound();

            var channel = await _context.Channels.FirstOrDefaultAsync(c => c.Id == id);
            var currentUser = await CookieService.CurrentUser(Request);
            if (channel == null || currentUser == null)
                return NotFound();

            var auth = await ChannelAuthService.UserIsAuthed(channel, currentUser);

            if (auth)
                return Redirect(Constants.Routes.EngageChannel + "/" + channel.Id.ToString());

            // User is not already authed, they need to enter the password
            ViewBag.Channel = channel;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AuthUserToChannel(Guid? id, ChannelAuth channelAuth)
        {
            if (id == null)
                return NotFound();

            var channel = await _context.Channels.FirstOrDefaultAsync(c => c.Id == id);
            var currentUser = await CookieService.CurrentUser(Request);
            if (channel == null || currentUser == null)
                return NotFound();

            var hashedInput = EncryptionService.Sha256(channelAuth.HashedKey);
            if (hashedInput == channel.KeyHash)
            {
                await ChannelAuthService.CreateAuth(channel, currentUser);
                return Redirect(Constants.Routes.EngageChannel + "/" + channel.Id);
            }

            ViewBag.Error = "Incorrect password";
            ViewBag.Channel = channel;
            return View();
        }
    }
}
