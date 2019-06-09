using System;
using System.Linq;
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
        public IActionResult Login()
        {
            if (CookieService.CurrentUser(_context, Request) != null)
                return Redirect(Constants.Routes.Home);

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
            var existingUser = _context.Users.Where(u => u.Name == user.Name &&
                                                    u.Password == user.Password).FirstOrDefault();


            if (existingUser == null)
                return View(); //TODO: show error as necessary here

            var userCookie = CookieService.GenerateUserCookie();
            Response.Cookies.Append(Constants.User.CookieKey, user.Name, userCookie);

            return Redirect(Constants.Routes.Home);
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
            if (newUser != null)
                return Redirect(Constants.Routes.Login);
            else
                return View(); //TODO: display relevant error
        }
    }
}
