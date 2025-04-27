using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Scene
{
    [Serializable]
    public class SceneNext
    {
        public string key { get; set; } = "";
        public string[]? next { get; set; }
    }
}
