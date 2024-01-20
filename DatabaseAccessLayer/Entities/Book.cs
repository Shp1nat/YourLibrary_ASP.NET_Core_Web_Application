namespace DatabaseAccessLayer.Entities
{
    public class Book
    {
        public int idBook { get; init; }
        public required Guid UidBook { get; init; }
        public required string Shifr { get; set; }
        public required string NameOfBook { get; set; }
        public ICollection<Author> AuthorsOfBook { get; set; }
        public ICollection<TypeBk> TypesBkOfBook { get; set; }
        public ICollection<Genre> GenresOfBook { get; set; }
        public ICollection<Example> ExamplesWithBook { get; set; }
    }
}