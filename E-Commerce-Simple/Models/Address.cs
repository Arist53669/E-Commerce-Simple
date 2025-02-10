namespace E_Commerce_Simple.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string CommuneOrWard { get; set; }
        public string SpecificAddress { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
