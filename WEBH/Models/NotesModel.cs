using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebH.Models
{
    public class Notes
    {
        public string LoginName { get; set; }

        [UIHint("Topic")]
        [Required(ErrorMessage = "Пожалуйста, введите тема.")]
        [StringLength(20, ErrorMessage = "Длина должна быть от 3 до 20 символов", MinimumLength = 3)]
        public string Topic { get; set; }

        [UIHint("Context")]
        [Required(ErrorMessage = "Пожалуйста, введите описание.")]
        [StringLength(300, ErrorMessage = "Длина должна быть от 3 до 300 символов", MinimumLength = 3)]
        public string Context { get; set; }

        public DateTime DataTime { get; set; }
    }

}

