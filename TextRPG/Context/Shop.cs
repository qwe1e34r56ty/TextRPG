using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Context
{
    public class Shop
    {
        public List<Item>? items { get; set; }
        public Shop(List<Item> items)
        {
            this.items = items;
        }
    }
}
