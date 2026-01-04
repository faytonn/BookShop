namespace Project.Api.Application.DTOs;

public record CategoryResponse
(
    Guid Id,
    string Name,
    int PriorityLevel,
    Guid? ParentId
 );

public record CategoryRequest
(
    string Name,
    int PriorityLevel,
    Guid? ParentId
);