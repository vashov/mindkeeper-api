namespace MindKeeper.Api.Data.Repositories.Users.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public string NormalizedMail { get; set; }
        public string PasswordHash { get; set; }
    }
}
