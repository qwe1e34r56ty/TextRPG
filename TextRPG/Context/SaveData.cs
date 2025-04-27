using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Context
{
    [Serializable]
    public class SaveData
    {
        public string? name { get; set; }
        public string? job { get; set; }
        public float attack { get; set; }
        public float guard { get; set; }
        public int hp { get; set; }
        public int gold { get; set; }
        public int clearCount {  get; set; }
        public Item[] items { get; set; } = System.Array.Empty<Item>();
        public Item[] shopItems { get; set; } = System.Array.Empty<Item>();
        public SaveData() { }
        public SaveData(GameContext gameContext)
        {
            name = gameContext.ch.name;
            job = gameContext.ch.job;
            attack = gameContext.ch.defaultAttack;
            guard = gameContext.ch.defaultGuard;
            hp = gameContext.ch.hp;
            gold = gameContext.ch.gold;
            clearCount = gameContext.ch.clearCount;
            items = gameContext.ch.inventory.items!.ToArray();
            shopItems = gameContext.shop.items!.ToArray();
        }
    }
}
