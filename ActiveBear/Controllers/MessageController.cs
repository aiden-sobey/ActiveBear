using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using ActiveBear.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ActiveBear.Controllers
{
    public class MessageController : Controller
    {
        private readonly ActiveBearContext _context;

        public MessageController(ActiveBearContext context)
        {
            _context = context;
        }

        // GET: /Message/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewAll()
        {
            ViewBag.Messages = _context.Message.ToList();
            ViewBag.Channels = _context.Channels.ToList();
            ViewBag.Users = _context.Users.ToList();

            ViewBag.Testing = 3;
            ViewBag.Iterations = 5;
            ViewBag.Message = "YEET";
            return View();
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var message = await _context.Message.FirstOrDefaultAsync(
                m => m.Id == id);

            if (message == null)
                return NotFound();
            else
                return View(message);
        }
    }
}
