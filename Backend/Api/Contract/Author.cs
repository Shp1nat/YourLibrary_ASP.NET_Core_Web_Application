namespace Backend.Api.Contract
{
    public class Author
    {
        public required Guid UidAuthor { get; init; }
        public required string NameOfAuthor { get; init; }
    }
}
