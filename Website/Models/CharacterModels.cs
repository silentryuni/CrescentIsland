using System.Data.Entity;

namespace CrescentIsland.Website.Models
{
    public class Character
    {

    }

    public partial class CharacterDbContext : DbContext
    {
        public CharacterDbContext()
            : base("LocalConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<Character> Characters { get; set; }
    }

    public class CharacterViewModel
    {
        public bool CharacterFound { get; set; }
        public bool Owner { get; set; }

        public string CharacterName { get; set; }
        public UserClass UserClass { get; set; }
        public int Level { get; set; }
        public int CurExp { get; set; }
        public int MaxExp { get; set; }
        public int Gold { get; set; }
        public int CurHealth { get; set; }
        public int MaxHealth { get; set; }
        public int CurEnergy { get; set; }
        public int MaxEnergy { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int MagicAttack { get; set; }
        public int MagicDefense { get; set; }
        public int Accuracy { get; set; }
        public int Evasion { get; set; }
    }
}