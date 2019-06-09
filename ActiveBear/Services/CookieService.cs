using System;
using ActiveBear.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ActiveBear.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

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

        public static User CurrentUser(ActiveBearContext context, HttpRequest request)
        {
            User currentUser = null;

            if (request.Cookies.ContainsKey(Constants.User.CookieKey))
            {
                try
                {
                    var requestCookie = Guid.Parse(request.Cookies[Constants.User.CookieKey]);
                    currentUser = context.Users.Where(u => u.CookieId == requestCookie).FirstOrDefault();
                }
                catch
                {
                    // Invalid cookie passed in
                }
            }

            return currentUser;
        }

        public static void DeleteUserCookie(HttpResponse response)
        {
            response.Cookies.Delete(Constants.User.CookieKey);
        }
    }
}
