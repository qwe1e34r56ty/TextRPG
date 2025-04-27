using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    public class ChoiceView : AView
    {
        private List<string> choices = new();
        public ChoiceView()
        {

        }

        public ChoiceView(ViewTransform v) : base(v)
        {
        }

        public override void ClearText()
        {
            this.choices.Clear();
        }

        public void SetText(string[] choices)
        {
            this.choices.Clear();
            foreach (var choice in choices)
            {
                this.choices.Add(choice);
            }
        }
        public override void Update()
        {
            view?.Clear();
            for (int i = 0; i < choices.Count; i++)
            {
                view?.WriteLine(i, choices[i]);
            }
        }
    }
}
