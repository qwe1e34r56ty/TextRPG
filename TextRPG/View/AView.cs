using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
   enum ViewEnum
    {
        script,
        dynamic,
        choice,
        log,
        sprite,
        input,
        viewCount
    }
    public static class ViewID{
        public const string Script = "Script";
        public const string Dynamic = "Dynamic";
        public const string Choice = "Choice";
        public const string Log = "Log";
        public const string Sprite = "Sprite";
        public const string Input = "Input";
    }
    internal abstract class AView
    {
        public ViewPort? view;
        public AView()
        {

        }
        public AView(ViewTransform v)
        {
            view = new(v);
        }
        public void setTransform(ViewTransform v)
        {
            view = new(v);
        }
        public abstract void Update();
        public void Render()
        {
            view?.Render();
        }
        public abstract void ClearText();
    }
}
