using Microsoft.EntityFrameworkCore;
using ProjectFlow.Application.Features.Projects;
using ProjectFlow.Domain.Entities;
using ProjectFlow.Domain.Enums;
using ProjectFlow.Infrastructure.Persistence;
using ProjectFlow.Infrastructure.Services;

namespace ProjectFlow.Tests.Unit;

public class ProjectMemberServiceTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddProjectMemberAsync_Throws_WhenUserIsNotWorkspaceMember()
    {
        using var db = CreateContext();
        var workspace = new Workspace { Id = Guid.NewGuid(), Name = "W", OwnerId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };
        var project = new Project { Id = Guid.NewGuid(), WorkspaceId = workspace.Id, Name = "P", CreatedAt = DateTime.UtcNow };
        db.Workspaces.Add(workspace);
        db.Projects.Add(project);
        await db.SaveChangesAsync();

        var service = new ProjectMemberService(db);
        var outsiderId = Guid.NewGuid();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.AddProjectMemberAsync(new AddProjectMemberRequest(project.Id, outsiderId)));
    }

    [Fact]
    public async Task AddProjectMemberAsync_Succeeds_WhenUserIsWorkspaceMember()
    {
        using var db = CreateContext();
        var workspace = new Workspace { Id = Guid.NewGuid(), Name = "W", OwnerId = Guid.NewGuid(), CreatedAt = DateTime.UtcNow };
        var project = new Project { Id = Guid.NewGuid(), WorkspaceId = workspace.Id, Name = "P", CreatedAt = DateTime.UtcNow };
        var userId = Guid.NewGuid();
        var membership = new WorkspaceMember { Id = Guid.NewGuid(), WorkspaceId = workspace.Id, UserId = userId, Role = WorkspaceRole.Member, JoinedAt = DateTime.UtcNow };
        db.Workspaces.Add(workspace);
        db.Projects.Add(project);
        db.WorkspaceMembers.Add(membership);
        await db.SaveChangesAsync();

        var service = new ProjectMemberService(db);
        var result = await service.AddProjectMemberAsync(new AddProjectMemberRequest(project.Id, userId));

        Assert.Equal(userId, result.UserId);
    }
}
