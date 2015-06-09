using System.ComponentModel.DataAnnotations;

namespace CrescentIsland.Website.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public ItemType Type { get; set; }
        [Required]
        public ItemQuality Quality { get; set; }
        public int LevelRequirement { get; set; }
        public int BuyCost { get; set; }
        public int SellCost { get; set; }
        public string Icon { get; set; }
    }

    public class UserItem
    {
        [Key]
        public int Id { get; set; }
        public int BagSlot { get; set; }
        public int EquipmentSlot { get; set; }

        [Required]
        public virtual Item Item { get; set; }
        [Required]
        public virtual Character Character { get; set; }
    }

    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ItemType Type { get; set; }
        public ItemQuality Quality { get; set; }
        public int LevelRequirement { get; set; }
        public string Icon { get; set; }
    }
    
    public enum ItemType
    {
        Item,
        Weapon,
        Headgear,
        Armor,
        Accessory,
        Spell
    }

    public enum ItemQuality
    {
        Common,
        Uncommon,
        Scarce,
        Rare,
        Legendary
    }
}