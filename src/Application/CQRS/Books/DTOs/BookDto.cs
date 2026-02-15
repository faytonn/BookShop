namespace Application.CQRS.Books.DTOs;

public sealed record BookRequest(
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    //List<AuthorRequest> Authors,
    IReadOnlyCollection<Guid> LanguageIds
);

public record struct BookDto(
    Guid Id,
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    //IEnumerable<AuthorResponse> Authors,
    IEnumerable<string?> Sellers
);