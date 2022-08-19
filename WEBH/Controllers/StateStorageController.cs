using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebH.Services;
using System;


namespace WebH
{
    public class StateStorageController : Controller
    {
        private const string sessionKey = ".Session";
        private const string cookieKey = ".MyAppV.Session";
        

        Visitors _visitors;


        public StateStorageController(Visitors visitors)
        {
            _visitors = visitors;
        }


        public IActionResult Visitors()
        {
            ViewBag.Cookies = Request.Cookies[cookieKey];
            ViewBag.SessionId = HttpContext.Session.Id;

            return View(_visitors.Count);
        }

        public void SetSession()
        {
            HttpContext.Session.SetString(sessionKey, HttpContext.Session.Id);
            string session = HttpContext.Session.GetString(sessionKey);
            _visitors.AddSession(session);
        }

        public void CheckSession()
        {
            string value = HttpContext.Session.GetString(sessionKey);
            if (value == null)
                SetSession();
            else
                _visitors.Check(value);
        }


        [HttpPost]
        public JsonResult CountkUser()
        {
            var value = _visitors.Count ;
            return Json(value);
        }

        [HttpPost]
        public JsonResult SessionUser()
        {
            CheckSession();
            return Json("Ok");
        }

    }
}
