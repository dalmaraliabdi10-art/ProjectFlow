using Microsoft.EntityFrameworkCore;
using ProjectFlow.Application.Features.Workspaces;
using ProjectFlow.Domain.Enums;
using ProjectFlow.Infrastructure.Persistence;
using ProjectFlow.Infrastructure.Services;

namespace ProjectFlow.Tests.Unit;

public class WorkspaceServiceTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateWorkspaceAsync_AddsOwnerAsMember()
    {
        using var db = CreateContext();
        var service = new WorkspaceService(db);
        var ownerId = Guid.NewGuid();

        var workspace = await service.CreateWorkspaceAsync(new CreateWorkspaceRequest("Acme", ownerId));

        var membership = await db.WorkspaceMembers.SingleAsync(m => m.WorkspaceId == workspace.Id);
        Assert.Equal(ownerId, membership.UserId);
        Assert.Equal(WorkspaceRole.Owner, membership.Role);
    }
}
