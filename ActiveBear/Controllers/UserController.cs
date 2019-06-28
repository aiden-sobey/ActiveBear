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
            if (CookieService.CurrentUser(Request) != null)
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
            CookieService.DeleteUserCookie(Response);
            var existingUser = _context.Users.Where(u => u.Name == user.Name &&
                                                    u.Password == user.Password).FirstOrDefault();

            if (existingUser == null)
            {
                ViewBag.Error = "Incorrect username/password";
                return View();
            }

            CookieService.GenerateUserCookie(existingUser, Response);

            return Redirect(Constants.Routes.Home);
        }

        [HttpPost]
        public IActionResult Register(User userRequest)
        {
            CookieService.DeleteUserCookie(Response);

            if (string.IsNullOrEmpty(userRequest.Name) ||
                string.IsNullOrEmpty(userRequest.Password))
            {
                ViewBag.Error = "Name/Password cannot be empty";
                return View();
            }
            
            if (_context.Users.Where(u => u.Name == userRequest.Name).Any())
            {
                ViewBag.Error = "A user with that name already exists!";
                return View();
            }

            var newUser = UserService.CreateUser(userRequest.Name, userRequest.Password, userRequest.Description);
            if (newUser != null)
                return Redirect(Constants.Routes.Login);

            // Something went wrong...
            ViewBag.Error = "An unknown error occured creating that user.";
            return View();
        }

        public IActionResult Logout()
        {
            CookieService.DeleteUserCookie(Response);
            return Redirect(Constants.Routes.Login);
        }
    }
}
