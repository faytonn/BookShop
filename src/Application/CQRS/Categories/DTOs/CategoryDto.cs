namespace Application.CQRS.Categories.DTOs;

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
