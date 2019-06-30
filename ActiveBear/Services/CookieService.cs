using System;
using ActiveBear.Models;
using System.Linq;
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
            User currentUser = null;
            var context = DbService.NewDbContext();

            if (!request.Cookies.ContainsKey(Constants.User.CookieKey))
                return currentUser;

            try
            {
                var requestCookie = Guid.Parse(request.Cookies[Constants.User.CookieKey]);
                currentUser = await context.Users.FirstOrDefaultAsync(u => u.CookieId == requestCookie);
            }
            catch
            {
                // Invalid cookie in the browser
                return currentUser;
            }

            return currentUser;
        }

        public static async Task<User> CurrentUser(string userCookie)
        {
            var context = DbService.NewDbContext();
            var userGuid = Guid.Empty;
            try
            {
                userGuid = Guid.Parse(userCookie);
                return await context.Users.FirstOrDefaultAsync(u => u.CookieId == userGuid);
            }
            catch
            {
                return null;
            }
        }

        public static void DeleteUserCookie(HttpResponse response)
        {
            response.Cookies.Delete(Constants.User.CookieKey);
        }
    }
}
