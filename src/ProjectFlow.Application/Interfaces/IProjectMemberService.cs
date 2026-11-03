using ProjectFlow.Application.Features.Projects;
using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Application.Interfaces;

public interface IProjectMemberService
{
    Task<ProjectMember> AddProjectMemberAsync(AddProjectMemberRequest request, CancellationToken ct = default);
}
