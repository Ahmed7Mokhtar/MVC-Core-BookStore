using System.ComponentModel.DataAnnotations;

namespace FirstConsoleApp.Enums
{
    public enum LanguageEnum
    {
        [Display(Name = "English Language")]
        English = 1,
        [Display(Name = "Arabic Language")]
        Arabic = 2,
        [Display(Name = "German Language")]
        German = 3,
        [Display(Name = "Spanish Language")] 
        Spanish = 4
    }
}
