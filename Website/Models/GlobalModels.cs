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
        public int Level { get; set; }
        public int CurHealth { get; set; }
        public int MaxHealth { get; set; }
        public int CurEnergy { get; set; }
        public int MaxEnergy { get; set; }
    }
}
