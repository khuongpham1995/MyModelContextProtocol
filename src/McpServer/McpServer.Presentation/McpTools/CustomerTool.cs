using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using McpServer.Application.Handlers.Customer;
using McpServer.Domain.Constants;
using McpServer.Domain.Entities;
using McpServer.Domain.Models;
using MediatR;
using ModelContextProtocol.Server;

namespace McpServer.Presentation.McpTools;

[McpServerToolType]
public sealed class CustomerTool(
    ISender sender,
    ILogger<CustomerTool> logger)
{
    [McpServerTool(Name = CommonConstant.GetCustomerToolName)]
    [Description(CommonConstant.GetCustomerToolDescription)]
    public async Task<ResponseModel<IReadOnlyCollection<Customer>>> GetCustomers()
    {
        var response = new ResponseModel<IReadOnlyCollection<Customer>>();
        try
        {
            var customers = await sender.Send(new GetCustomersQuery());
            if (customers.Count == 0)
                response.Fail(CommonConstant.NoCustomersFoundMessage);
            else
                response.Succeed(customers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonConstant.ErrorRetrieveCustomerData);
            response.Fail(CommonConstant.NoCustomersFoundMessage);
        }

        return response;
    }

    [McpServerTool(Name = CommonConstant.GetCustomerByNameToolName)]
    [Description(CommonConstant.GetCustomerByNameToolDescription)]
    public async Task<ResponseModel<IReadOnlyCollection<Customer>>> GetCustomersByName(
        [Description(CommonConstant.CustomerNamePropertyDescription)]
        string? name
    )
    {
        var response = new ResponseModel<IReadOnlyCollection<Customer>>();
        try
        {
            var customers = await sender.Send(new GetCustomersByNameQuery(name));
            response.Succeed(customers);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonConstant.ErrorRetrieveCustomerData);
            response.Fail(CommonConstant.NoCustomersFoundMessage);
        }

        return response;
    }

    [McpServerTool(Name = CommonConstant.CreateCustomerToolName)]
    [Description(CommonConstant.CreateCustomerToolDescription)]
    public async Task<ResponseModel<Customer>> CreateCustomer(
        [Description(CommonConstant.CustomerNamePropertyDescription)]
        string? name,
        [Description(CommonConstant.CustomerAddressPropertyDescription)]
        string? address,
        [Description(CommonConstant.CustomerEmailPropertyDescription)]
        string? email,
        [Description(CommonConstant.CustomerPhonePropertyDescription)]
        string? phone,
        [Description(CommonConstant.CustomerAvatarPropertyDescription)]
        string? avatar
    )
    {
        var response = new ResponseModel<Customer>();
        try
        {
            await sender.Send(new CreateCustomerCommand(name, email, phone, address, avatar));
            response.Succeed();
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, CommonConstant.ErrorCustomerNotCreated);
            response.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonConstant.ErrorCustomerNotCreated);
            response.Fail(CommonConstant.ErrorCustomerNotCreated);
        }

        return response;
    }

    [McpServerTool(Name = CommonConstant.UpdateCustomerToolName)]
    [Description(CommonConstant.UpdateCustomerToolDescription)]
    public async Task<ResponseModel<Customer>> UpdateCustomer(
        [Description(CommonConstant.CustomerIdPropertyDescription)]
        int id,
        [Description(CommonConstant.CustomerNamePropertyDescription)]
        string? name,
        [Description(CommonConstant.CustomerAddressPropertyDescription)]
        string? address,
        [Description(CommonConstant.CustomerEmailPropertyDescription)]
        string? email,
        [Description(CommonConstant.CustomerPhonePropertyDescription)]
        string? phone,
        [Description(CommonConstant.CustomerAvatarPropertyDescription)]
        string? avatar
    )
    {
        var response = new ResponseModel<Customer>();
        try
        {
            await sender.Send(new UpdateCustomerCommand(name, email, phone, address, avatar) { Id = id });
            response.Succeed();
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, CommonConstant.ErrorCustomerNotUpdated);
            response.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonConstant.ErrorCustomerNotUpdated);
            response.Fail(CommonConstant.ErrorCustomerNotUpdated);
        }

        return response;
    }

    [McpServerTool(Name = CommonConstant.DeleteCustomerToolName)]
    [Description(CommonConstant.DeleteCustomerToolDescription)]
    public async Task<ResponseModel<Customer>> DeleteCustomer(
        [Description(CommonConstant.CustomerIdPropertyDescription)]
        int id
    )
    {
        var response = new ResponseModel<Customer>();
        try
        {
            await sender.Send(new DeleteCustomerCommand(id));
            response.Succeed();
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, CommonConstant.ErrorCustomerNotDeleted);
            response.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonConstant.ErrorCustomerNotDeleted);
            response.Fail(CommonConstant.ErrorCustomerNotDeleted);
        }

        return response;
    }
}