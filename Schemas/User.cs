using TestScimRest.Attributes;

namespace TestScimRest.Schemas
{
    public class User: IResource
    {
        public string[] Schemas { get; set; } = { "urn:ietf:params:scim:schemas:core:2.0:User" };

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Name Name { get; set; }

        public List<Email> Emails { get; set; }

        public string DisplayName { get; set; }
        public string Locale { get; set; }
        public string ExternalId { get; set; }
        public List<Group> Groups { get; set; }
        public bool Active { get; set; }
        public Meta Meta
        {
            get
            {
                return new Meta
                {
                    ResourceType = "User"
                };
            }
        }
    }
}
