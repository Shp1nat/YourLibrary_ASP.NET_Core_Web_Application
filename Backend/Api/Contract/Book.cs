namespace Backend.Api.Contract
{
    public class Book
    {
        public required Guid UidBook { get; init; }
        public required string Shifr { get; init; }
        public required string NameOfBook { get; init; }
        public required List<string> Authors { get; init; }
        public required List<string> TypesBk { get; init; }
        public required List<string> Genres { get; init; }
    }
}
