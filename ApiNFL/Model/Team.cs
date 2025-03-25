namespace ApiNFL.Model
{
    public class Team
    {
        public int TeamID { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Stadium { get; set; }
        public int FoundationYear { get; set; }
        public string? Conference { get; set; }
    }
}
