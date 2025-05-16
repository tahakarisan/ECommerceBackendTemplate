using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidations
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Description).MinimumLength(2);

            RuleFor(p => p.StockQuantity).NotEmpty();
            RuleFor(p => p.Price).GreaterThan(0);
            //RuleFor(p => p.Price).GreaterThanOrEqualTo(10).When(p => p.BrandId == 1);
            //RuleFor(p => p.Name).Must(StartWithA);
            // RuleFor(p => p.Name.StartsWith("A"));
        }
        private bool StartWithA(string arg)
        {
            return arg.StartsWith("A");
        }
    }
}
