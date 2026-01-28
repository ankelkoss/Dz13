namespace MvcP1.Models
{
    public class ContactsModel : BaseModel
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? PhoneAlt { get; set; }
        public string? Email { get; set; }
        public string? DescShort { get; set; }
    }
}
