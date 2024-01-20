namespace DatabaseAccessLayer.Entities
{
    public class Author
    {
        public int idAuthor { get; init; }
        public required Guid UidAuthor { get; init; }
        public required string NameOfAuthor { get; set; }
        public ICollection<Book> BooksWithAuthor { get; set; }
    }
}