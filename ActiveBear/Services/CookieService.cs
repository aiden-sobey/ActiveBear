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
        public static CookieOptions GenerateUserCookie()
        {
            var cookieOption = new CookieOptions();
            cookieOption.IsEssential = true;
            cookieOption.Expires = DateTime.Now.AddDays(7);
            return cookieOption;
        }

        public static User CurrentUser(ActiveBearContext context, HttpRequest request)
        {
            User currentUser = null;

            if (request.Cookies.ContainsKey(Constants.User.CookieKey))
            {
                var userName = request.Cookies[Constants.User.CookieKey];
                currentUser = context.Users.Where(u => u.Name == userName).FirstOrDefault();
            }

            return currentUser;
        }
    }
}
