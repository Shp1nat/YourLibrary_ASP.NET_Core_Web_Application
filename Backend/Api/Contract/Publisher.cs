namespace Backend.Api.Contract
{
    public class Publisher
    {
        public required Guid UidPublisher { get; init; }
        public required string NameOfPublisher { get; init; }
    }
}