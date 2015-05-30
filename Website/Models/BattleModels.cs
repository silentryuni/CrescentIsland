using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrescentIsland.Website.Models
{
    public class BattleModel
    {
        public bool Success { get; set; }

        public int CurHealthChange { get; set; }
        public int CurEnergyChange { get; set; }
    }
}