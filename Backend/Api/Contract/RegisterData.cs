namespace Backend.Api.Contract
{
    public class RegisterData
    {
        public required string Login { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
    }
}
