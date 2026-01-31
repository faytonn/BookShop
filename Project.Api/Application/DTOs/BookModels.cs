namespace Project.Api.Application.DTOs;


// sellerin bir basa objecti gelsin - adi, idsi, sekili ve s

public record struct BookResponse(
    Guid Id,
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    IEnumerable<AuthorResponse> Authors,
    IEnumerable<string?> Sellers
);

public record struct BookSellerDTO
    (

    );

public record struct BookOrderDTO
    (
    Guid Id,
    string Name,
    decimal Price,
    int Stock
    );

public record struct BookDetailedResponse(
    Guid Id,
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    bool IsReleased,
    IEnumerable<string?> Sellers,
    IEnumerable<AuthorResponse> Authors,
    IEnumerable<string?> Languages
);

public record struct BookRequest(
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    List<AuthorRequest> Authors,
    IReadOnlyCollection<Guid> LanguageIds
);

public record struct UpdateBookRequest(
    Guid Id,
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    List<AuthorRequest> Authors,
    IReadOnlyCollection<Guid> LanguageIds
);