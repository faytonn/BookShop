namespace Project.Api.Application.DTOs;

public record struct BookResponse(
    Guid Id,
    string Name,
    decimal Price,
    decimal Discount,
    List<string> Sellers
);

public record struct BookDetailedResponse(
    Guid Id,
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    bool IsReleased,
    List<string> Sellers,
    List<string> Languages
);

public record struct BookRequest(
    string Name,
    decimal Price,
    byte Discount,
    DateTime ReleaseDate,
    IReadOnlyCollection<Guid> LanguageIds
);