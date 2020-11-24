using System;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Controllers
{
    public class ChannelController : Controller
    {
        private readonly ActiveBearContext context;

        public ChannelController(ActiveBearContext _context)
        {
            context = _context;
        }

        public async Task<IActionResult> Engage(Guid? id)
        {
            if (id == null)
                return NotFound();

            var channel = await context.Channels.FirstOrDefaultAsync(x => x.Id == id);
            if (channel == null)
                return NotFound();

            // Check the current user is authorised
            var cookie = new CookieService(context);
            var currentUser = await cookie.CurrentUser(Request);
            if (currentUser == null)
                return Redirect(Constants.Routes.Login);

            var channelAuth = new ChannelAuthService(context);
            var auth = await channelAuth.UserIsAuthed(channel, currentUser);
            if (!auth)
                return Redirect(Constants.Routes.AuthUserToChannel + id.ToString());

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult lightbulb()
        {
            return View();
        }

        [HttpGet]
        public IActionResult XluBtQi()
        {
            ViewBag.Success = "";
            return View();
        }

        [HttpPost]
        public IActionResult XluBtQi(string title)
        {
            if (title == "42069")
            {
                ViewBag.Success = "true";
                ViewBag.Result = "Congratulations!!! Message Aiden the text 'lvl5ezNub' and you have won the race!";
            }
            else
                ViewBag.Success = "Wrong";

            return View();
        }
    }
}
