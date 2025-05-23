using FluentValidation;
using McpServer.Domain.Interfaces;
using MediatR;

namespace McpServer.Application.CustomerHandlers;

public record DeleteCustomerCommand(int Id) : IRequest;

public class DeleteCustomerHandler(ICustomerApiClient customerApiClient) : IRequestHandler<DeleteCustomerCommand>
{
    public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerApiClient.GetByIdAsync(request.Id, cancellationToken);
        await customerApiClient.DeleteAsync(customer!.Id, cancellationToken);
    }
}

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Customer Id must be greater than 0.");
    }
}