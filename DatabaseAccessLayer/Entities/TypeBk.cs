namespace DatabaseAccessLayer.Entities
{
    public class TypeBk
    {
        public int idTypeBk { get; init; }
        public required Guid UidTypeBk { get; init; }
        public required string NameOfTypeBk { get; set; }
        public ICollection<Book> BooksWithTypeBk { get; set; }
    }
}