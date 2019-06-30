using System;
using System.Linq;
using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Login()
        {
            var currentUser = await CookieService.CurrentUser(Request);
            if (currentUser != null)
                return Redirect(Constants.Routes.Home);

            return View();
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            CookieService.DeleteUserCookie(Response);
            var existingUser = await _context.Users.FirstOrDefaultAsync
                (u => u.Name == user.Name && u.Password == user.Password);

            if (existingUser == null)
            {
                ViewBag.Error = "Incorrect username/password";
                return View();
            }

            CookieService.GenerateUserCookie(existingUser, Response);

            return Redirect(Constants.Routes.Home);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User userRequest)
        {
            CookieService.DeleteUserCookie(Response);

            if (string.IsNullOrEmpty(userRequest.Name) ||
                string.IsNullOrEmpty(userRequest.Password))
            {
                ViewBag.Error = "Name/Password cannot be empty";
                return View();
            }

            if (userRequest.Password.Length < 12)
            {
                ViewBag.Error = "Password must be at least 12 characters!";
                return View();
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == userRequest.Name);
            if (existingUser != null)
            {
                ViewBag.Error = "A user with that name already exists!";
                return View();
            }

            var newUser = await UserService.CreateUser(userRequest.Name, userRequest.Password, userRequest.Description);
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
