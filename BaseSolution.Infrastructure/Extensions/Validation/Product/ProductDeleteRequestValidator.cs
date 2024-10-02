using BaseSolution.Application.DataTransferObjects.Product.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validation.Product
{
    public class ProductDeleteRequestValidator : AbstractValidator<ProductDeleteRequest>
    {
        public ProductDeleteRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product Id is required.");
        }
    }
}
