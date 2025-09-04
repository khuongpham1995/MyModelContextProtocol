using FluentValidation;
using McpServer.Domain.Interfaces;
using MediatR;

namespace McpServer.Application.Handlers.Customer;

public record CreateCustomerCommand(
    string? Name,
    string? Email,
    string? Phone = null,
    string? Address = null,
    string? Avatar = null) : IRequest;

public class CreateCustomerHandler(ICustomerApiClient customerApiClient) : IRequestHandler<CreateCustomerCommand>
{
    public async Task Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        var customer = new Domain.Entities.Customer();
        customer.Create(
            command.Name!,
            command.Email!,
            command.Phone,
            command.Address,
            command.Avatar
        );
        await customerApiClient.CreateAsync(customer, cancellationToken);
    }
}

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email address is required.");
    }
}