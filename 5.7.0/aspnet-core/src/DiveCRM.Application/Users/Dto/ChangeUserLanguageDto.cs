using System.ComponentModel.DataAnnotations;

namespace DiveCRM.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}