namespace CrescentIsland.Website.Models
{
    public class GlobalModel
    {
        public CurrentUserModel CurrentUser { get; set; }
    }

    public class CurrentUserModel
    {
        public string AvatarUrl { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public string Level { get; set; }
        public string CurHealth { get; set; }
        public string MaxHealth { get; set; }
        public string CurEnergy { get; set; }
        public string MaxEnergy { get; set; }
    }
}
