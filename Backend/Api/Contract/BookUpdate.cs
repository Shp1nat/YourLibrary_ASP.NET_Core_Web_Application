namespace Backend.Api.Contract
{
    public class BookUpdate
    {
        public required string Shifr { get; init; }
        public required string NameOfBook { get; init; }
        public required List<string> AuthorsOfBook { get; init; }
        public required List<string> TypesBkOfBook { get; init; }
        public required List<string> GenresOfBook { get; init; }
    }
}
