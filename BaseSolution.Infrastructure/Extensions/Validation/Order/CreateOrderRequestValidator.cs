using BaseSolution.Application.DataTransferObjects.Order.OrderRequest;
using BaseSolution.Infrastructure.Extensions.Validation.OrderItem;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseSolution.Infrastructure.Extensions.Validation.Order
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.")
                .MustAsync(async (id, cancellationToken) => await CustomerExists(id))
                .WithMessage("Customer does not exist.");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("OrderDate is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemCreateRequestValidator());
        }

        private async Task<bool> CustomerExists(int customerId)
        {
            return await Task.FromResult(true); 
        }
    }
}
