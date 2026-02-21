namespace Application.CQRS.Books.DTOs;

public record struct AuthorRequest
(
    string Name
);

public record struct AuthorResponse
(
    Guid Id,
    string Name
);
