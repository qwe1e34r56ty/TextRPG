using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Scene
{
    [Serializable]
    public class SceneText
    {
        public string key { get; set; } = "";
        public string[]? scriptText { get; set; }
        public string[]? choiceText { get; set; }
        public string[]? spriteText { get; set; }
    }
}
