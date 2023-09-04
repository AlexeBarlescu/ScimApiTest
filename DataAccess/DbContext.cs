using System.Collections.Generic;
using System.Linq;
using TestScimRest.Schemas;

namespace TestScimRest.DataAccess
{
    public class DbContext
    {
        private readonly List<User> Users;

        private readonly Dictionary<Guid, Group> Groups;

        private readonly Dictionary<Guid, List<User>> userGroupAssignments;

        public DbContext()
        {

            var joe = new User
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                ExternalId = "00ujl29u0le5T6Aj10h7",
                UserName = "john.doe@example.com",
                Email = "john.doe@example.com",
                LastName = "Doe",
                FirstName = "John",
                DisplayName = "John Doe",
                Active = true,
                Locale = "en-US"
            };
            var jane = new User
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                ExternalId = "00ujl29u0le5T6Aj10h8",
                UserName = "jane.smith@example.com",
                Email = "jane.smith@example.com",
                LastName = "Smith",
                FirstName = "Jane",
                DisplayName = "Jane Smith",
                Active = true,
                Locale = "en-GB"
            };
            var bob = new User
            {
                Id = new Guid("33333333-3333-3333-3333-333333333333"),
                ExternalId = "00ujl29u0le5T6Aj10h9",
                UserName = "bob.johnson@example.com",
                Email = "bob.johnson@example.com",
                LastName = "Johnson",
                FirstName = "Bob",
                DisplayName = "Bob Johnson",
                Active = true,
                Locale = "fr-FR"
            };

            Users = new List<User> { joe, jane, bob };


            var admin = new Group()
            {
                Id = new Guid("11111111-1111-1111-4444-111111111111"),
                Name = "Admin"
            };

            var guest = new Group()
            {
                Id = new Guid("22222222-2222-2222-4444-222222222222"),
                Name = "Guest"
            };

            var builder = new Group()
            {
                Id = new Guid("33333333-3333-3333-4444-333333333333"),
                Name = "Builder"
            };

            Groups = new Dictionary<Guid, Group>();
            Groups.Add(admin.Id, admin);
            Groups.Add(guest.Id, guest);
            Groups.Add(builder.Id, builder);

            // Initialize groups
            userGroupAssignments = new Dictionary<Guid, List<User>>();
            AddToGroup(joe, admin);
            AddToGroup(jane, guest);
            AddToGroup(bob, guest);
            AddToGroup(bob, builder);
        }

        public Group GetGroup(Guid id)
        {
            return Groups[id];
        }
        public Group GetGroup(string name)
        {
            return Groups.Values.FirstOrDefault(g => g.Name == name);
        }


        public void AddToGroup(User user, Group group)
        {
            if (!userGroupAssignments.ContainsKey(group.Id))
            {
                userGroupAssignments[group.Id] = new List<User>();
            }

            if (!userGroupAssignments[group.Id].Contains(user))
            {
                userGroupAssignments[group.Id].Add(user);
            }
        }

        public void RemoveFromGroup(User user, Guid groupId)
        {
            if (userGroupAssignments.ContainsKey(groupId))
            {
                userGroupAssignments[groupId].Remove(user);
            }
        }

        public List<User> GetUsersInGroup(Guid groupId)
        {
            if (userGroupAssignments.ContainsKey(groupId))
            {
                return userGroupAssignments[groupId];
            }

            return new List<User>();
        }

        public List<Group> GetUserGroups(User user)
        {
            List<Group> userGroups = new List<Group>();

            foreach (var kvp in userGroupAssignments)
            {
                if (kvp.Value.Contains(user))
                {
                    userGroups.Add(GetGroup(kvp.Key));
                }
            }
            return userGroups;
        }

        public List<Group> GetAllGroups()
        {
            return Groups.Values.ToList();
        }

        public IEnumerable<User> AllUsers()
        {
            return Users;
        }

        public IEnumerable<User> GetUsers(int pageIndex, int count)
        {
            if (pageIndex < 1 || count < 1)
            {
                throw new ArgumentException("Invalid pageIndex or count values.");
            }

            int startIndex = (pageIndex - 1) * count;
            int endIndex = startIndex + count;

            if (startIndex >= Users.Count)
            {
                return new List<User>(); // Return an empty list if the startIndex is out of bounds.
            }

            // Ensure endIndex does not exceed the list length.
            endIndex = Math.Min(endIndex, Users.Count);

            // Use LINQ to retrieve the subset of users based on the pageIndex and count.
            return Users.Skip(startIndex).Take(endIndex - startIndex).ToList();
        }

        public User GetUser(Guid id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUser(string userName)
        {
            return Users.FirstOrDefault(user => user.UserName == userName);
        }


        public User GetUser(int id)
        {
            return Users[id];
        }

        public User CreateUser(Schemas.CreateUserRequest request)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Email = request.Emails.First().Value,
                FirstName = request.Name.GivenName,
                LastName = request.Name.FamilyName,
                DisplayName = request.DisplayName,
                ExternalId = request.ExternalId,
                Active = request.Active,
                Locale = request.Locale
            };

            Users.Add(user);

            if(request.Groups.Count > 0)
            {
                foreach(var item in request.Groups)
                {
                    var group = GetGroup(item.Name); 
                    if(group == null)
                    {
                        group.Id = Guid.NewGuid();
                        Groups.Add(group.Id, group);
                    }

                    AddToGroup(user, group);             
                }
            }

            return user;
        }

        public List<Group> GetGroups(int pageIndex, int count)
        {
            if (pageIndex < 1 || count < 1)
            {
                throw new ArgumentException("Invalid pageIndex or count values.");
            }

            List<Group> groupList = Groups.Values.ToList();

            int startIndex = (pageIndex - 1) * count;
            int endIndex = startIndex + count;

            if (startIndex >= groupList.Count)
            {
                return new List<Group>(); // Return an empty list if the startIndex is out of bounds.
            }

            // Ensure endIndex does not exceed the list length.
            endIndex = Math.Min(endIndex, groupList.Count);

            // Use LINQ to retrieve the subset of groups based on the pageIndex and count.
            return groupList.Skip(startIndex).Take(endIndex - startIndex).ToList();
        }
    }
}
