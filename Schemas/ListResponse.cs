using System.Reflection.Metadata.Ecma335;

namespace TestScimRest.Schemas
{
    public class ListResponse<T> where T : IResource
    {
        public string[] Schemas { get; set; } = { "urn:ietf:params:scim:api:messages:2.0:ListResponse" };
        public int TotalResults { get; set; }
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public List<T> Resources { get; set; }
    }
}
