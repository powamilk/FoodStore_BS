using BaseSolution.Application.DataTransferObjects.Example.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validation.Category
{
    public class CategoryDeleteRequestValidator : AbstractValidator<CategoryDeleteRequest>
    {
        public CategoryDeleteRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
