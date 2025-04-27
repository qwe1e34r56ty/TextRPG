using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    [Serializable]
    public class ViewTransform
    {
        public string key { get; set; } = "";
        public int x { get; set; }
        public int y { get; set; }
        public int width {  get; set; }
        public int height { get; set; }
        public int border {  get; set; }
        public int borderCharAscii { get; set; }
    }
}
