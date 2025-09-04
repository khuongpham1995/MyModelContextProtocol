using FluentValidation;
using McpServer.Domain.Interfaces;
using MediatR;

namespace McpServer.Application.Handlers.Customer;

public record UpdateCustomerCommand(
    string? Name,
    string? Email,
    string? Phone = null,
    string? Address = null,
    string? Avatar = null) : IRequest
{
    public int Id { get; set; }
}

public class UpdateCustomerHandler(ICustomerApiClient customerApiClient) : IRequestHandler<UpdateCustomerCommand>
{
    public async Task Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerApiClient.GetByIdAsync(request.Id, cancellationToken);
        customer!.Update(
            request.Name!,
            request.Email!,
            request.Phone,
            request.Address,
            request.Avatar
        );
        await customerApiClient.UpdateAsync(request.Id, customer, cancellationToken);
    }
}

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Customer Id must be greater than 0.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email address is required.");
    }
}