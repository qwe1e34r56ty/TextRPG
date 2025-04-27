using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    public class InputView : AView
    {
        public String prefix { get; private set; }

        public InputView() : base()
        {
            this.prefix = " >> ";
        }

        public override void ClearText()
        {
            
        }

        public InputView(ViewTransform v) : base(v)
        {
            this.prefix = " >> ";
        }
        public override void Update()
        {
            view?.Clear();
            view?.WriteLine(0, " 원하시는 행동을 입력해주세요.");
            view?.WriteLine(1, " r = 화면 복구, q = 게임 종료");
            view?.WriteLine(2, prefix);

        }
        public (int? x, int? y) GetCursorPosition()
        {
            return (view?.x + prefix?.Length, view?.y);
        }
        public void SetCursor()
        {
            int x = (view?.border + view?.x ?? 0) + prefix.Length;
            int y = (view?.border * 2 + view?.y + 1 ?? 0);
            Console.SetCursorPosition(x, y);
        }
    }
}
