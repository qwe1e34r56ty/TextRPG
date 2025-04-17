using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    internal class ViewPort
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int border { get; private set; }
        private string[] buffer;
        public ViewPort(ViewTransform v)
        {
            this.x = v.x;
            this.y = v.y;
            this.width = v.width;
            this.height = v.height;
            buffer = new string[height - border * 2];
            this.border = v.border;
        }
        public void Clear()
        {
            for(int i = 0; i< height - border * 2; i++)
            {
                buffer[i] = new string(' ', width - border * 2);
            }
        }
        public void WriteLine(int row, string text)
        {
            if (row < 0 || row >= height) return;
            buffer[row] = PadRightWide(text, width - border * 2);
        }
        public void Render()
        {
            Console.SetCursorPosition(x + border, y);
            Console.Write(new string('=', width - border * 2));
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("=");
            }
            for(int i = 0; i < buffer.Length; i++)
            {
                Console.SetCursorPosition(x + 1, y + i + 1);
                Console.Write(buffer[i] ?? new string(' ', width - border * 2));
            }
            Console.SetCursorPosition(x + border, y + height - border);
            Console.Write(new string('=', width - border * 2));
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x + width - border, y + i);
                Console.Write("=");
            }
            Console.WriteLine("");
        }
        public static int GetConsoleWidth(string text)
        {
            int width = 0;
            foreach (var ch in text)
            {
                if (char.GetUnicodeCategory(ch) == UnicodeCategory.OtherLetter || ch > 0x1100)
                    width += 2; // 한글, 한자 등 전각
                else
                    width += 1;
            }
            return width;
        }
        public static string PadRightWide(string text, int totalWidth)
        {
            int actualWidth = GetConsoleWidth(text);
            return text + new string(' ', Math.Max(0, totalWidth - actualWidth));
        }
    }
}
