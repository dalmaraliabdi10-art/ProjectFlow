using ProjectFlow.Application.Features.Workspaces;
using ProjectFlow.Application.Interfaces;
using ProjectFlow.Domain.Entities;
using ProjectFlow.Domain.Enums;
using ProjectFlow.Infrastructure.Persistence;

namespace ProjectFlow.Infrastructure.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly AppDbContext _db;

    public WorkspaceService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Workspace> CreateWorkspaceAsync(CreateWorkspaceRequest request, CancellationToken ct = default)
    {
        var workspace = new Workspace
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            OwnerId = request.OwnerId,
            CreatedAt = DateTime.UtcNow
        };

        var ownerMembership = new WorkspaceMember
        {
            Id = Guid.NewGuid(),
            WorkspaceId = workspace.Id,
            UserId = request.OwnerId,
            Role = WorkspaceRole.Owner,
            JoinedAt = DateTime.UtcNow
        };

        _db.Workspaces.Add(workspace);
        _db.WorkspaceMembers.Add(ownerMembership);

        await _db.SaveChangesAsync(ct);

        return workspace;
    }
}
