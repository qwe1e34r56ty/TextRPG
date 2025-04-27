using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Context
{
    [Serializable]
    public class Character
    {
        public string? name { get; set; }
        public string? job { get; set; }
        public float defaultAttack { get; set; } // 현재 레벨 기본 공격력
        public float defaultGuard { get; set; } // 현재 레벨 기본 방어력
        public int hp { get; set; }
        public int gold { get; set; }
        public int clearCount {  get; set; }
        public Inventory inventory
        { get; set; }

        public Character(SaveData saveData)
        {
            this.name = saveData.name;
            this.job = saveData.job;
            this.defaultAttack = saveData.attack;
            this.defaultGuard = saveData.guard;
            this.hp = saveData.hp;
            this.gold = saveData.gold;
            this.clearCount = saveData.clearCount;
            this.inventory = new Inventory(new List<Item>(saveData.items));
        }

        public Character(string name, string job, float attack, float guard, int hp, int gold, int clearCount, Inventory inventory)
        {
            this.name = name;
            this.job = job;
            this.defaultAttack = attack;
            this.defaultGuard = guard;
            this.hp = hp;
            this.gold = gold;
            this.clearCount = clearCount;
            this.inventory = inventory;
        }

        public int getLevel()
        {
            if(clearCount >= 10)
            {
                return 5;
            }else if(clearCount >= 6)
            {
                return 4;
            }else if(clearCount >= 3)
            {
                return 3;
            }else if(clearCount >= 1)
            {
                return 2;
            }else
            {
                return 1;
            }
        }

        public float getNoWeaponAttack()
        {
            return defaultAttack + (getLevel() - 1) * 0.5f;
        }

        public float getNoArmorGuard()
        {
            return defaultGuard + (getLevel() - 1) * 1.0f;
        }

        public int getPlusAttack()
        {
            int plusAttack = 0;
            foreach(var item in inventory.items!)
            {
                if (item.equiped == true)
                {
                    plusAttack += item.attack;
                }
            }
            return plusAttack;
        }
        public int getPlusGuard()
        {
            int plusGuard = 0;
            foreach (var item in inventory.items!)
            {
                if (item.equiped == true)
                {
                    plusGuard += item.guard;
                }
            }
            return plusGuard;
        }

        public float getTotalAttack()
        {
            return getNoWeaponAttack() + getPlusAttack();
        }

        public float getTotalGuard()
        {
            return getNoArmorGuard() + getPlusGuard();
        }
    }
}
