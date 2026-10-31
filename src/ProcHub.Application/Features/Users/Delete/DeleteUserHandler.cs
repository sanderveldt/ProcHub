using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProcHub.Application.Abstractions;
using ProcHub.Application.Exceptions;

namespace ProcHub.Application.Features.Users.Delete;

public sealed class DeleteUserHandler(
    IUserIdentityService identityService,
    IProcHubDbContext dbContext,
    ILogger<DeleteUserHandler> logger)
{
    public async Task HandleAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ValidationException(
                "UserId cannot be 0 or below.");
        }

        var user = await identityService
            .GetByIdAsync(
                id,
                cancellationToken);

        bool isInUse = await dbContext
            .Suppliers.AnyAsync(s =>
                s.ResponsibleBuyerId == user.Id,
                cancellationToken);
        
        if (isInUse)
        {
            throw new ResourceInUseException(
                "This user is still attached to a supplier.");
        }
        
        await identityService.DeleteAsync(
            user.Id,
            cancellationToken);

        logger.LogInformation(
            "User {UserId} has been deleted.",
            user.Id);
    }
}