namespace E5XVC8.Models;

public record Rental(
    int Id,
    int CarId,
    DateTime RentedAt,
    DateTime? ReturnedAt
);