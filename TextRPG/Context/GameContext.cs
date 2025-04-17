using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Context
{
    [Serializable]
    internal class GameContext
    {
        public Character ch { get; set; }
        public Shop shop { get; set; }
        public List<DungeonData> dungeonList { get; set; } = new List<DungeonData>();
        public DungeonData? enteredDungeon { get; set; } = null;
        public int prevHp { get; set; }
        public int curHp {  get; set; }
        public int prevGold { get; set; }
        public int curGold {  get; set; }
        public GameContext(SaveData saveData, List<DungeonData> dungeonData)
        {
            ch = new(saveData);
            shop = new(new List<Item>(saveData.shopItems));
            this.dungeonList = new List<DungeonData>(dungeonData);
        }
    }
}
