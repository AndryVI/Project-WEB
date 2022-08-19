using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebH.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebH
{
    public class MenuController: Controller
    {
        public IActionResult Menu()
        {
            return View();
        }
        public IActionResult Timer()
        {
            return View();
        }


        public IActionResult GetIpAndBrowser()
        {
            string secchua = HttpContext.Request.Headers["sec-ch-ua"];
            var browser = secchua.Split(',').Select(a => a.Replace(@"""", "")).ToArray();
            var result = Regex.Replace(browser[1],
                                    @" (?<brow>\w+ \w+);v=(?<vars>\d+)",
                                    @"${brow}, Version: ${vars}");


            string pubIp = new System.Net.WebClient().DownloadString("https://api.ipify.org");

            ViewBag.Text = result;

            ViewData["browser"] = result;
            ViewData["ip"] = pubIp;

            return View();
        }


        public IActionResult Form()
        { 
            return View();
        }




        [HttpPost]
        public IActionResult Form(GetForm getForm)
        {
            Debug.WriteLine("First = " + getForm.First);
            Debug.WriteLine("Second = " + getForm.Second);
            Debug.WriteLine("Count = " + getForm.Count);

            return View(getForm);
        }
    }
}
