using WebH.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebH.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;


namespace WebH.Controllers
{
    public class FilterController : Controller
    {
        private const string cookieKey = ".MyAppV.Session";
        private const string cookieKeyWrite = ".CookiesM";

        private static Dictionary<string, DateTime> cookieKeySave = new Dictionary<string, DateTime>();

        public IActionResult GetUniqueUsers(CountUsersAttribute countUsers )
        {
            //ViewBag.Cookies = Request.Cookies[cookieKey];
            ViewBag.CountUsers = countUsers.CountUniqueUsers;
            return View();
        }

        public IActionResult LogUserAction(WriterAndReadService writerAndRead)
        {
            var listlog = writerAndRead.GetLog();

            return View(listlog);
        }


        #region Cookies

        
        public IActionResult Cookies()
        {
            return View();
        }

        public IActionResult ShowCookies()
        {
            bool check = false;
            ViewBag.Cookies = Request.Cookies[cookieKeyWrite];
            foreach (var item in cookieKeySave)
            {
                if (item.Key == Request.Cookies[cookieKeyWrite])
                {
                    check = true;
                    ViewBag.CookiesTime = item.Value;
                }
            }
            if (!check)
            {
                ViewBag.Cookies = "Cookies not found";
                ViewBag.CookiesTime = "Time not found";
            }

            return View();
        }


        [HttpPost]
        public IActionResult AddCookies(string value, DateTime timeExpires)
        {
            CookieOptions options = new CookieOptions();
            //options.Expires = DateTime.Now.AddMinutes(1);
            options.Expires = timeExpires;

            Response.Cookies.Append(cookieKeyWrite, value, options);


            bool check = false;
            foreach (var item in cookieKeySave)
                if (item.Key == value)
                    check = true;

            if (!check)
                cookieKeySave.Add(value, timeExpires);
            else
            {
                cookieKeySave.Remove(value);
                cookieKeySave.Add(value, timeExpires);
            }
            return LocalRedirect($"~/Filter/ShowCookies/");
        }

        #endregion
    }
}
 