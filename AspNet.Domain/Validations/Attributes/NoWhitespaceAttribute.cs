using System.ComponentModel.DataAnnotations;

namespace AspNet.Domain.Validations.Attributes
{
    public class NoWhitespaceAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        { 
            if (value == null)
            { 
                return true; 
            }

            if (value is not string text)
                return false;
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
