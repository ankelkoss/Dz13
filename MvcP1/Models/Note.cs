namespace MvcP1.Models
{
    public class Note : Base
    {
        public string Name { get; set; } = null!;
        public string Text { get; set; } = null!;
        public List<string> Tags { get; set; } = new();
    }
}
