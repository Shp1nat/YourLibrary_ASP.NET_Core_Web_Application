namespace DatabaseAccessLayer.Entities
{
    public class Example
    {
        public int idExample { get; init; }
        public required Guid UidExample { get; init; }

        public required Book Book { get; set; }
        public required Publisher Publisher { get; set; }
        public required int YearOfCreation { get; set; }
        public bool IsTaken { get; set; }
        public ICollection<Order> OrdersWithExample { get; init; }
    }
}