namespace ProjectFlow.Application.Features.Projects;

public record AddProjectMemberRequest(Guid ProjectId, Guid UserId);
