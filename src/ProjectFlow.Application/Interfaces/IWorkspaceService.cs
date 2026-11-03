using ProjectFlow.Application.Features.Workspaces;
using ProjectFlow.Domain.Entities;

namespace ProjectFlow.Application.Interfaces;

public interface IWorkspaceService
{
    Task<Workspace> CreateWorkspaceAsync(CreateWorkspaceRequest request, CancellationToken ct = default);
}
