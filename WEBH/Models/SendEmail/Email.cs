using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WebH.Models
{
    public class Email
    {

        [Required(ErrorMessage = "Пожалуйста, введите валидный адрес электронной почты получителя.")]
        [EmailAddress]
        [UIHint("RecipientEmail")]
        public string RecipientEmail { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите  тему.")]
        [StringLength(50, ErrorMessage = "Длина должна быть от 5 до 50 символов", MinimumLength = 5)]
        [UIHint("Topic")]
        public string Topic { get; set; }
    }


    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }

}
