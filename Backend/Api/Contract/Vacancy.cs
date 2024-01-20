namespace Backend.Api.Contract
{
    public class Vacancy
    {
        public required Guid UidVacancy { get; init; }
        public required string Text { get; init; }
        public required User User { get; init; }
        public required int StatusOfVacancy { get; init; }
    }
}