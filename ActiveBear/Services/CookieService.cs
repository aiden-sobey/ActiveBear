using System;
using ActiveBear.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

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

        public static User CurrentUser(HttpRequest request)
        {
            User currentUser = null;

            if (request.Cookies.ContainsKey(Constants.User.CookieKey))
            {
                try
                {
                    var requestCookie = Guid.Parse(request.Cookies[Constants.User.CookieKey]);
                    currentUser = DbService.NewDbContext().Users.Where(u => u.CookieId == requestCookie).FirstOrDefault();
                }
                catch
                {
                    // Invalid cookie in the browser
                }
            }

            return currentUser;
        }

        public static User CurrentUser(string userCookie)
        {
            var context = DbService.NewDbContext();
            var userGuid = Guid.Empty;
            try
            {
                userGuid = Guid.Parse(userCookie);
                return context.Users.FirstOrDefault(u => u.CookieId == userGuid);
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
