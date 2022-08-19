using System.ComponentModel.DataAnnotations;

namespace WebH.Models
{
    public enum EnumLanguageType
    {
        [Display(Name = "JavaScript")]
        JavaScript,
        [Display(Name = "C#")]
        C,
        Java,
        Python,
        [Display(Name = "Основы")]
        Basic
    }
}
