namespace Backend.Api.Contract
{
    public class UserUpdate
    {
        public required string Email { get; init; }
        public required string? Name { get; init; }
        public required string? Surname { get; init; }
        public required string? Otchestvo { get; init; }
        public required string Login { get; init; }
        public required string Password { get; init; }
        public required string? Address { get; init; }
        public required string? PhoneNumber { get; init; }
        public required int? Age { get; init; }
    }
}
