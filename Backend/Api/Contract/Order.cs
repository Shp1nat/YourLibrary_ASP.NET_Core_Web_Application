namespace Backend.Api.Contract
{
    public class Order
    {
        public required Guid UidOrder { get; init; }
        public required Example Example { get; init; }
        public required User User { get; init; }
        public required bool IsBack { get; init; }
    }
}
