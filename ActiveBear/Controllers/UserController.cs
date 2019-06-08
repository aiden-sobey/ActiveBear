using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActiveBear.Controllers
{
    public class UserController : Controller
    {
        private readonly ActiveBearContext _context;

        public UserController(ActiveBearContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User userRequest)
        {
            // TODO: handle proper errors here
            if (_context.Users.Where(u => u.Name == userRequest.Name).Any())
                return NotFound();

            if (String.IsNullOrEmpty(userRequest.Name) ||
                String.IsNullOrEmpty(userRequest.Password))
                return NotFound();

            var newUser = UserService.CreateUser(userRequest.Name, userRequest.Password, userRequest.Description, _context);
            ViewBag.SignupStatus = "Successful";
            if (newUser != null)
                return Redirect("/User/Login");
            else
                return View(); //TODO: display relevant error
        }
    }
}
