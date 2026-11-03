namespace ProjectFlow.Application.Features.Workspaces;

public record CreateWorkspaceRequest(string Name, Guid OwnerId);
