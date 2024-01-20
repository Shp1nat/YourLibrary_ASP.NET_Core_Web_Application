namespace DatabaseAccessLayer.Entities
{
    public class Order
    {
        public int idOrder { get; init; }
        public required Guid UidOrder { get; init; }
        public required Example Example { get; init; }
        public required User User { get; init; }
        public required bool IsBack { get; set; }
    }
}