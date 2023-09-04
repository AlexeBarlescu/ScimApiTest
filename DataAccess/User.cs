namespace TestScimRest.DataAccess
{
    public class User
    {
        public Guid Id { get; set; }
        public string ExternalId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string LastName { get; internal set; }
        public string FirstName { get; internal set; }
        public string DisplayName { get; set; }
        public bool Active { get; set; }
        public string Locale { get; set; }
    }
}
