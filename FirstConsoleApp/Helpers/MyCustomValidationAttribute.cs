using System.ComponentModel.DataAnnotations;

namespace FirstConsoleApp.Helpers
{
    // implements ValidationAttribute class
    public class MyCustomValidationAttribute : ValidationAttribute
    {
        public MyCustomValidationAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        // override ValidationResult method
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value != null)
            {
                string bookName = value.ToString();
                if(bookName.Contains(Text))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(ErrorMessage ?? "add book after book name!");
        }
    }
}
