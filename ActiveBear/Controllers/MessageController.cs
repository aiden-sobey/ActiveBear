using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;
using ActiveBear.Services;

namespace ActiveBear.Controllers
{
    public class MessageController : Controller
    {
        private readonly ActiveBearContext _context;

        public MessageController(ActiveBearContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ViewAll()
        {
            var currentUser = await CookieService.CurrentUser(Request);

            if (currentUser == null)
                return Redirect(Constants.Routes.Login);

            ViewBag.CurrentUser = currentUser;
            ViewBag.Messages = _context.Messages.ToList();
            ViewBag.Channels = _context.Channels.ToList();
            ViewBag.Users = _context.Users.ToList();

            return View();
        }
    }
}
