using System;
using System.ComponentModel.DataAnnotations;

namespace WebH.Models
{
    public class RegistrationModelForAnAppointment
    {
        [Required(ErrorMessage = "Пожалуйста, введите  имя.")]
        [StringLength(50, ErrorMessage = "Длина должна быть от 2 до 50 символов", MinimumLength = 2)]
        [UIHint("FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите фамилию.")]
        [StringLength(50, ErrorMessage = "Длина должна быть от 4 до  50 символов", MinimumLength = 4)]
        [UIHint("SureName")]
        public string SureName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите валидный адрес электронной почты.")]
        [EmailAddress]
        [UIHint("EmailAddress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите дата консультации")]
        [UIHint("DateOfConsultation")]
        public DateTime? DateOfConsultation { get; set; }
    }

}