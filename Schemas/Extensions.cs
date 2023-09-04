using System.Runtime.CompilerServices;

namespace TestScimRest.Schemas
{
    public static class Extensions
    {
        public static User ToSchemaUser(this DataAccess.User user)
        {
            return new User() { 
                Id = user.Id,
                ExternalId = user.ExternalId,
                UserName = user.UserName,
                Name = new Attributes.Name
                {
                    FamilyName = user.LastName,
                    GivenName = user.FirstName
                },
                Emails = new List<Attributes.Email>
                {
                    new Attributes.Email{ Primary = true, Type = "email", Value = user.Email }
                },
                Active = user.Active,
                DisplayName = user.DisplayName,
                Locale = user.Locale,
                Groups = new List<Group>()
            };
        }
    }
}
