using BaseSolution.Application.DataTransferObjects.Example.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validation.Category
{
    public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(225).WithMessage("Name must not exceed 225 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(225).WithMessage("Description must not exceed 225 characters.");
        }
    }
}
