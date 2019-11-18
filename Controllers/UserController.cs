using System.Threading.Tasks;
using ActiveBear.Models;
using ActiveBear.Services;
using Microsoft.AspNetCore.Mvc;

namespace ActiveBear.Controllers
{
    public class UserController : Controller
    {
        private readonly ActiveBearContext context;
        private CookieService cookie;
        private UserService userService;

        public UserController(ActiveBearContext _context)
        {
            context = _context;
            cookie = new CookieService(context);
            userService = new UserService(context);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var currentUser = await cookie.CurrentUser(Request);
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
            cookie.DeleteUserCookie(Response);

            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Error = "All fields are mandatory";
                return View();
            }

            var existingUser = await userService.ExistingUser(user.Name, user.Password);

            if (existingUser == null)
            {
                ViewBag.Error = "Incorrect username/password";
                return View();
            }

            cookie.GenerateUserCookie(existingUser, Response);

            return Redirect(Constants.Routes.Home);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User userRequest)
        {
            cookie.DeleteUserCookie(Response);

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

            var existingUser = await userService.ExistingUser(userRequest.Name);
            if (existingUser != null)
            {
                ViewBag.Error = "A user with that name already exists!";
                return View();
            }

            var newUser = await userService.CreateUser(userRequest.Name, userRequest.Password, userRequest.Description);
            if (newUser != null)
                return Redirect(Constants.Routes.Login);

            // Something went wrong...
            ViewBag.Error = "An unknown error occured creating that user.";
            return View();
        }

        public IActionResult Logout()
        {
            cookie.DeleteUserCookie(Response);
            return Redirect(Constants.Routes.Login);
        }
    }
}
