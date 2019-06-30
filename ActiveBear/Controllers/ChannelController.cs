using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Controllers
{
    public class ChannelController : Controller
    {
        private readonly ActiveBearContext _context;

        public ChannelController(ActiveBearContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Engage(Guid? id)
        {
            if (id == null)
                return NotFound();

            var channel = await _context.Channels.FirstOrDefaultAsync(x => x.Id == id);
            if (channel == null)
                return NotFound();

            // Check the current user is authorised
            var currentUser = await CookieService.CurrentUser(Request);
            if (currentUser == null)
                return Redirect(Constants.Routes.Login);
            var auth = await ChannelAuthService.UserIsAuthed(channel, currentUser);
            if (!auth)
                return Redirect(Constants.Routes.AuthUserToChannel + id.ToString());

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
