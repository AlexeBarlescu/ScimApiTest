namespace TestScimRest.Schemas
{
    public class ErrorResponse
    {
        public string[] Schemas { get; set; } = { "urn:ietf:params:scim:api:messages:2.0:Error" };
        public string Detail { get; set; }
        public string Status { get; set; }
    }
}
