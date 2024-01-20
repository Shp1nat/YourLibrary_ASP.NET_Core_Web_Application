namespace Backend.Api.Contract
{
    public class Genre
    {
        public required Guid UidGenre { get; init; }
        public required string NameOfGenre { get; init; }
    }
}
