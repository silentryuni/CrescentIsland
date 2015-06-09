using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual ICollection<UserItem> Items { get; set; }
    }
    
    public class CharacterUserViewModel
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

    public class CharacterViewModel
    {
        public bool UserFound { get; set; }
        public bool CharacterFound { get; set; }
        public bool OwnCharacter { get; set; }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string Country { get; set; }
        public string Biography { get; set; }
        public byte[] AvatarImage { get; set; }
        public string AvatarMimeType { get; set; }
        public string AccountAge { get; set; }
        public string LastLogin { get; set; }

        public string CharacterName { get; set; }
        public string UserClass { get; set; }
        public int Level { get; set; }
        public int CurExp { get; set; }
        public int MaxExp { get; set; }
        public string Gold { get; set; }
        public int CurHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int MagicAttack { get; set; }
        public int MagicDefense { get; set; }
        public int Accuracy { get; set; }
        public int Evasion { get; set; }
    }

    public class InventoryViewModel
    {
        public InventoryItem[] Equipment { get; set; }
        public InventoryItem[] Bag { get; set; }
    }

    public class UpdateCharacterInventory
    {
        public int ItemId { get; set; }
        public int BagSlot { get; set; }
        public int EquipmentSlot { get; set; }

        public int SecondItemId { get; set; }
        public int SecondBagSlot { get; set; }
        public int SecondEquipmentSlot { get; set; }
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