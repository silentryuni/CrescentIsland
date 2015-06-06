using System.ComponentModel.DataAnnotations;

namespace CrescentIsland.Website.Models
{
    public class Character
    {
        [Key]
        [Required]
        [StringLength(128)]
        public string Id { get; set; }
        [Required]
        [StringLength(256)]
        public string CharacterName { get; set; }
        [Required]
        public UserClass UserClass { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public int CurExp { get; set; }
        [Required]
        public int MaxExp { get; set; }
        [Required]
        public int Gold { get; set; }
        [Required]
        public int CurHealth { get; set; }
        [Required]
        public int MaxHealth { get; set; }
        [Required]
        public int CurEnergy { get; set; }
        [Required]
        public int MaxEnergy { get; set; }
        [Required]
        public int Attack { get; set; }
        [Required]
        public int Defense { get; set; }
        [Required]
        public int MagicAttack { get; set; }
        [Required]
        public int MagicDefense { get; set; }
        [Required]
        public int Accuracy { get; set; }
        [Required]
        public int Evasion { get; set; }
    }
    
    public class CharacterViewModel
    {
        public bool CharacterFound { get; set; }
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

    public class CharacterUserViewModel
    {

    }

    public enum UserClass
    {
        Valkyrie,
        Warrior,
        Sorceress,
        Rogue,
        Engineer,
        Samurai
    }
}