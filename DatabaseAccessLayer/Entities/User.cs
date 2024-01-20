namespace DatabaseAccessLayer.Entities
{
    public class User
    {
        public int idUser { get; init; }
        public required Guid UidUser { get; init; }
        public required string Email { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Otchestvo { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Age { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Order> OrdersOfUser { get; init; }
        public ICollection<Vacancy> Vacancies { get; init; }
    }
}