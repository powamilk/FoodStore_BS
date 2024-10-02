using BaseSolution.Application.DataTransferObjects.OrderItem.OrderItemRequest;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validation.OrderItem
{
    public class DeleteOrderItemRequestValidator : AbstractValidator<DeleteOrderItemRequest>
    {
        public DeleteOrderItemRequestValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("OrderId is required.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");
        }
    }
}
