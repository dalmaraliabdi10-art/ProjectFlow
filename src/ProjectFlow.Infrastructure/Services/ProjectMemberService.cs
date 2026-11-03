using Microsoft.EntityFrameworkCore;
using ProjectFlow.Application.Features.Projects;
using ProjectFlow.Application.Interfaces;
using ProjectFlow.Domain.Entities;
using ProjectFlow.Infrastructure.Persistence;

namespace ProjectFlow.Infrastructure.Services;

public class ProjectMemberService : IProjectMemberService
{
    private readonly AppDbContext _db;

    public ProjectMemberService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ProjectMember> AddProjectMemberAsync(AddProjectMemberRequest request, CancellationToken ct = default)
    {
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, ct)
            ?? throw new InvalidOperationException($"Project '{request.ProjectId}' was not found.");

        var isWorkspaceMember = await _db.WorkspaceMembers.AnyAsync(
            m => m.WorkspaceId == project.WorkspaceId && m.UserId == request.UserId, ct);

        if (!isWorkspaceMember)
            throw new InvalidOperationException(
                $"User '{request.UserId}' must be a member of the project's workspace before being added to the project.");

        var member = new ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = request.ProjectId,
            UserId = request.UserId,
            AddedAt = DateTime.UtcNow
        };

        _db.ProjectMembers.Add(member);
        await _db.SaveChangesAsync(ct);

        return member;
    }
}
