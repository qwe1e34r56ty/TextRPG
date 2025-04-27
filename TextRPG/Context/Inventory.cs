using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Context
{
    [Serializable]
    public class Inventory
    {
        public List<Item>? items{ get; set; }
        public Inventory(List<Item> items)
        {
            this.items = items;
        }
    }
}


