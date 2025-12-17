using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UI.classes
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = ErrorStrings.RequiredName)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorStrings.InvalidName)]
        public string Name { get; set; }
        
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorStrings.InvalidSecondName)]
        public string SecondName { get; set; }
        [Required(ErrorMessage = ErrorStrings.RequiredLastName)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorStrings.InvalidLastName)]
        public string LastName { get; set; }
        [Required(ErrorMessage = ErrorStrings.RequiredLogin)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorStrings.InvalidLogin)]
        public string Login { get; set; }
        public string Password { get; set; }
        public Occupation Occupation { get; set; }
        [Required(ErrorMessage = ErrorStrings.RequiredEmail)]
        [EmailAddress(ErrorMessage = ErrorStrings.InvalidEmail)]
        public string Email { get; set; }
        public string FullName { get; set; }

        public List<ValidationResult> Validate()
        {
            var context = new ValidationContext(this);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, results, true);
            return results;
        }
    }
}
