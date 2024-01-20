namespace DatabaseAccessLayer.Entities
{
    public class Vacancy
    {
        public int idVacancy { get; init; }
        public required Guid UidVacancy { get; init; }
        public required string Text { get; init; }
        public int StatusOfVacancy { get; set; }
        public required User User { get; init; }
    }
}