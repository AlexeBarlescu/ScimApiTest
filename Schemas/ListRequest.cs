namespace TestScimRest.Schemas
{
    public class ListRequest
    {
        public int StartIndex { get; set; } //Index of the page
        public int Count { get; set; }
    }
}
