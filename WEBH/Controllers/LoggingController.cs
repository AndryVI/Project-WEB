using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using WebH.Models;
using Microsoft.Extensions.Logging;
using WebH.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebH.Controllers
{
    public class LoggingController : Controller
    {
        private readonly IMailService mailService;
        //Seq
        private readonly ILogger<HomeController> logger;
        public IConfiguration Configuration { get; }


        public LoggingController(IConfiguration configuration, ILogger<HomeController> logger, IMailService mailService)
        {
            this.mailService = mailService;

            Configuration = configuration;

            //Seq
            this.logger = logger;
            logger.LogInformation("This is test information message from LoggingController");
        }


        #region Logging


        [Authorize(Roles = "Admin")]
        public IActionResult SeqLogging()
        {
            logger.LogInformation("This is test information message for Task-4 ");
            logger.LogInformation("This is test information message with semantic logging. {date} and {value} for Task-4 ", DateTime.Now, 123);
            logger.LogError(new NullReferenceException(), "Logged exception");
            logger.LogWarning("This is test LogWarning message for Task-4 ");

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult LoggingToFile(WriterAndReadService writerAndRead)
        {
            logger.LogInformation("This is test information message for Task-3");
            logger.LogError(new AppDomainUnloadedException(), "Logged exception (LoggingToFile)");

            ViewBag.Text = "Логи добавлены";

            var readfile = writerAndRead.FileRead(getfile: @"\logs-20220813.0.txt", getdirectory: @"Logs");


            return View(readfile.Text);
        }

        #endregion

        #region ConfigurationFromJson

        public IActionResult ReloadChangeConfiguration()
        {
            ViewBag.Text = Configuration["Task-1"];            
            return View();
        }

        public IActionResult GetConfiguration()
        {
            ViewBag.Text = Configuration["Task-5"];
            return View();
        }

        #endregion


        public IActionResult SendEmail()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SendEmail(Email model)
        {
            if (ModelState.IsValid)
            {
                _ = SendMail(new MailRequest() { ToEmail = model.RecipientEmail, Subject = model.Topic });
                ViewBag.ResultSend = 1;
                return View(model);
            }
            else
                return View();
        }


        public async Task<IActionResult> SendMail(MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }


        #region Cache
        /*
           [HttpPost]
        public IActionResult SendEmail(Email model)
        {
            if (ModelState.IsValid)
            {
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress("rachel.schaden86@ethereal.email");
                //    mail.To.Add("dimander123@gmail.com");
                //    mail.Subject = "Hello World";
                //    mail.Body = "<h1>Hello</h1>";
                //    mail.IsBodyHtml = true;
                //    //mail.Attachments.Add(new Attachment("C:\\file.zip"));

                //    using (SmtpClient smtp = new SmtpClient("smtp.ethereal.email", 587))
                //    {
                //        smtp.Credentials = new NetworkCredential("rachel.schaden86@ethereal.email", "ftCZBjCXD7eyHjY29E");
                //        smtp.EnableSsl = true;
                //        //smtp.UseDefaultCredentials = false;
                //        smtp.Send(mail);
                //    }
                //}


                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Rachel Schaden", "rachel.schaden86@ethereal.email"));
                emailMessage.To.Add(new MailboxAddress("Tes", "dimander123@gmail.com"));
                emailMessage.Subject = "Test";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "text test"
                };


                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.ethereal.email", 587, false);
                    client.Authenticate("rachel.schaden86@ethereal.email", "ftCZBjCXD7eyHjY29E");
                    client.Send(emailMessage);
                    client.Disconnect(true);
                }


                var set = SendMail(new MailRequest() { ToEmail = model.RecipientEmail, Subject = model.Topic });
                ViewBag.Result = 1;
                return View(model);
            }
            else
                return View();

        }

         
         */

        #endregion
    }
}
