using TestScimRest.Attributes;

namespace TestScimRest.Schemas
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public Name Name { get; set; }

        public List<Email> Emails { get; set; }

        public string DisplayName { get; set; }
        public string Locale { get; set; }
        public string ExternalId { get; set; }
        public List<Group> Groups { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
    }
}
