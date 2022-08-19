using Microsoft.AspNetCore.Mvc;
using WebH.Services;

namespace WebH
{
    public class CalcController : Controller
    {
        CalcService _calcService;

        public CalcController(CalcService calcService)
        {
            _calcService = calcService;
        }

        public IActionResult Calc(double? id)
        {
            ViewBag.Result = id;
            return View();
        }


        [HttpPost]
        public JsonResult Add(double a, double b)
        {
            return Json(_calcService.Add(a, b));
        }

        [HttpPost]
        public JsonResult Sub(double a, double b)
        {
            var result = _calcService.Sub(a, b);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Mul(double a, double b)
        {
            return Json(_calcService.Mul(a, b));
        }

        [HttpPost]
        public JsonResult Div(double a, double b)
        {
            return Json(_calcService.Div(a, b));
        }

    }
}