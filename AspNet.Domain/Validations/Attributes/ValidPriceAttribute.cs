using System.ComponentModel.DataAnnotations;

namespace AspNet.Domain.Validations.Attributes
{
    public class ValidPriceAttribute(double min, double max) : ValidationAttribute
    {
        private readonly decimal _min = (decimal)min;
        private readonly decimal _max = (decimal)max;

        public override bool IsValid(object? value)
        {
            if (_min > _max)
                return false;

            if (value == null)
                return true;

            if (value is not decimal price)
                return false;

            return price >= _min && price <= _max;
        }
    }
}