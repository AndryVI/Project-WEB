using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System;
using WebH.Services;



namespace WebH.Infrastructure
{
    public class LogAction : ActionFilterAttribute
    {
        private const string cookieKey = ".MyAppV.Session";
        private WriterAndReadService wArSeirvice = new WriterAndReadService();
        private Stopwatch timer;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            timer = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            timer.Stop();

            var cookies = context.HttpContext.Request.Cookies[cookieKey];
            var datetime = DateTime.Now;

            var dfs = timer.Elapsed;

            if (cookies is not null)
            {
                wArSeirvice.FileWriter($"{context.ActionDescriptor.DisplayName}; {datetime} ; {cookies} ; {timer.Elapsed}", getfile: @"\logActionList.txt", getdirectory: @"App_Data");
            }
        }
    }
}
