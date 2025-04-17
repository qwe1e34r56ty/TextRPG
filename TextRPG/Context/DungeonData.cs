using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Context
{
    [Serializable]
    internal class DungeonData
    {
        public string title { get; set; } = "";
        public float recommandArmor { get; set; }
        public int reward {  get; set; }
        public DungeonData(string title, float recommandArmor, int reward)
        {
            this.title = title;
            this.recommandArmor = recommandArmor;
            this.reward = reward;
        }
    }
}
