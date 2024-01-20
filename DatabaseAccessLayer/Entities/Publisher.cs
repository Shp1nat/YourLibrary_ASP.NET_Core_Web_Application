namespace DatabaseAccessLayer.Entities
{
    public class Publisher
    {
        public int idPublisher { get; init; }
        public required Guid UidPublisher { get; init; }
        public required string NameOfPublisher { get; set; }
        public ICollection<Example> ExamplesPublished { get; init; }
    }
}