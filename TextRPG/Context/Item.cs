using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Context
{
    [Serializable]
    internal class Item
    {
        public string key { get; set; }
        public bool equiped { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public int price {  get; set; }
        public int attack { get; set; }
        public int guard { get; set; }
        public bool bought { get; set; }

        public bool armor {  get; set; }
        public bool weapon {  get; set; }
        public Item(string key, bool equiped, string? name, string? description, int price, int attack, int guard, bool bought, bool armor, bool weapon)
        {
            this.key = key;
            this.equiped = equiped;
            this.name = name;
            this.description = description;
            this.price = price;
            this.attack = attack;
            this.guard = guard;
            this.bought = bought;
            this.armor = armor;
            this.weapon = weapon;
        }
    }
}
