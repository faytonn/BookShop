namespace Project.Api.Application.DTOs;

public record struct BookResponse(
    Guid Id,
    string Name,
    decimal Price,
    byte Discount,
    IEnumerable<string?> Sellers
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
    IEnumerable<string?> Languages
);

public record struct BookRequest(
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    IReadOnlyCollection<Guid> LanguageIds
);