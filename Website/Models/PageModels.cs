using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace CrescentIsland.Website.Models
{
    public class HeaderModel
    {
        public byte[] AvatarImage { get; set; }
        public string AvatarMimeType { get; set; }
        public ICollection<IdentityUserRole> UserRoles { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int CurHealth { get; set; }
        public int MaxHealth { get; set; }
        public int CurEnergy { get; set; }
        public int MaxEnergy { get; set; }
    }
}