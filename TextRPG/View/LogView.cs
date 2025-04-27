using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    public class LogView : AView
    {
        private Queue<string> logs = new();

        public override void ClearText()
        {
            this.logs.Clear();
        }
        public void AddLog(string line)
        {
            logs.Enqueue(line);
            if (logs.Count > view?.height - 2) logs.Dequeue();
        }
        public override void Update()
        {
            view?.Clear();
            var arr = logs.ToArray();
            for(int i = 0; i < arr.Length; i++)
            {
                view?.WriteLine(i, arr[i]);
            }
        }
    }
}
