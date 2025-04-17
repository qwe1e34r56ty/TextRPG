using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    [Serializable]
    internal class ViewTransform
    {
        public ViewTransform(string key, int x, int y, int width, int height, int border)
        {
            this.key = key;
            this.x = x;
            this.y = y;
            this.width = width;      
            this.height = height;
            this.border = border;
        }

        public string key { get; set; } = "";
        public int x { get; set; }
        public int y { get; set; }
        public int width {  get; set; }
        public int height { get; set; }
        public int border {  get; set; }
    }
}
