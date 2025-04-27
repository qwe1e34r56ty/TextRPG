using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    public class ScriptView : AView
    {
        private List<string> lines = new();

        public override void ClearText()
        {
            this.lines.Clear();
        }
        public void SetText(string[] lines)
        {
            this.lines.Clear();
            foreach (var line in lines)
            {
                this.lines.Add(line);
            }
        }
        public override void Update()
        {
            view?.Clear();
            for (int i = 0; i < lines.Count; i++)
            {
                view?.WriteLine(i, lines[i]);
            }
        }
    }
}
