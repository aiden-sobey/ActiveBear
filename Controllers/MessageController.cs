using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;
using ActiveBear.Services;

namespace ActiveBear.Controllers
{
    public class MessageController : Controller
    {
        private readonly ActiveBearContext context;

        public MessageController(ActiveBearContext _context)
        {
            context = _context;
        }

        public async Task<IActionResult> ViewAll()
        {
            var cookie = new CookieService(context);
            var currentUser = await cookie.CurrentUser(Request);

            if (currentUser == null)
                return Redirect(Constants.Routes.Login);

            ViewBag.CurrentUser = currentUser;
            ViewBag.Channels = await context.Channels.ToListAsync();

            return View();
        }
    }
}
