namespace Backend.Api.Contract
{
    public class NewExample
    {
        public required Guid UidBook { get; init; }
        public required Guid UidPublisher { get; init; }
        public required int YearOfCreation { get; init; }
    }
}
