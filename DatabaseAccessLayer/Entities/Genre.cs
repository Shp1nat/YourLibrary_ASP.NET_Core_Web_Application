namespace DatabaseAccessLayer.Entities
{
    public class Genre
    {
        public int idGenre { get; init; }
        public required Guid UidGenre { get; init; }
        public required string NameOfGenre { get; set; }
        public ICollection<Book> BooksWithGenre { get; set; }
    }
}