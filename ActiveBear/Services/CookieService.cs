using System;
using ActiveBear.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ActiveBear.Services
{
    public class CookieService : BaseService
    {
        public CookieService(ActiveBearContext _context) : base(_context) { }

        public void GenerateUserCookie(User user, HttpResponse response)
        {
            var cookieOption = new CookieOptions();
            cookieOption.IsEssential = true;
            cookieOption.Expires = DateTime.Now.AddDays(7);

            response.Cookies.Delete(Constants.User.CookieKey);
            response.Cookies.Append(Constants.User.CookieKey, user.CookieId.ToString(), cookieOption);
        }

        public async Task<User> CurrentUser(HttpRequest request)
        {
            if (!request.Cookies.ContainsKey(Constants.User.CookieKey))
                return null;

            try
            {
                var requestCookie = Guid.Parse(request.Cookies[Constants.User.CookieKey]);
                var userService = new UserService(context);
                return await userService.ExistingUser(requestCookie);
            }
            catch
            {
                return null;    // Invalid cookie in the browser
            }
        }

        public void DeleteUserCookie(HttpResponse response)
        {
            response.Cookies.Delete(Constants.User.CookieKey);
        }
    }
}
