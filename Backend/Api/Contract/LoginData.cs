namespace Backend.Api.Contract
{
    public class LoginData
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
