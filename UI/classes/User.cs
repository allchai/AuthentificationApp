using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Windows.Documents;

namespace UI.classes
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Имя\" обязательно к заполнению")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Слишком короткое или слишком длинное имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле \"Фамилия\" обязательно к заполнению")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Слишком короткая или слишком длинная фамилия")]
        public string SecondName { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Слишком короткое или слишком длинное отчество")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Поле \"Логин\" обязательно к заполнению")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Слишком короткий или слишком длинный логин")]
        public string Login { get; set; }
        public string Password { get; set; }
        public Occupation Occupation { get; set; }
        [Required(ErrorMessage = "Поле \"Email\" обязательно к заполнению")]
        [EmailAddress]
        public string Email { get; set; }
        public string FullName { get; set; }

        public static List<ValidationResult> Validate(User user)
        {
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(user, context, results, true);
            return results;
        }
    }
}
