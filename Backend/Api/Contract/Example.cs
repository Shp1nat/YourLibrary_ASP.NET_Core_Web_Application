namespace Backend.Api.Contract
{
    public class Example
    {
        public required Guid UidExample { get; init; }
        public required Book Book { get; init; }
        public required Publisher Publisher { get; init; }
        public required int YearOfCreation { get; init; }
        public required bool IsTaken { get; init; }
    }
}
