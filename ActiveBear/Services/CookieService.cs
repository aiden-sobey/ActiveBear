using System;
using ActiveBear.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ActiveBear.Services
{
    public static class CookieService
    {
        public static void GenerateUserCookie(User user, HttpResponse response)
        {
            var cookieOption = new CookieOptions();
            cookieOption.IsEssential = true;
            cookieOption.Expires = DateTime.Now.AddDays(7);

            response.Cookies.Delete(Constants.User.CookieKey);
            response.Cookies.Append(Constants.User.CookieKey, user.CookieId.ToString(), cookieOption);
        }

        public static async Task<User> CurrentUser(HttpRequest request)
        {
            if (!request.Cookies.ContainsKey(Constants.User.CookieKey))
                return null;

            try
            {
                var requestCookie = Guid.Parse(request.Cookies[Constants.User.CookieKey]);
                return await UserService.ExistingUser(requestCookie);
            }
            catch
            {
                return null;    // Invalid cookie in the browser
            }
        }

        public static void DeleteUserCookie(HttpResponse response)
        {
            response.Cookies.Delete(Constants.User.CookieKey);
        }
    }
}
