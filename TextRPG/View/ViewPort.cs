using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.View
{
    public class ViewPort
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int border { get; set; }
        public char borderChar {  get; set; }
        private string[] buffer;
        public const char Blank = ' ';
        public ViewPort(ViewTransform v)
        {
            this.x = v.x;
            this.y = v.y;
            this.width = v.width;
            this.height = v.height;
            buffer = new string[height - border * 2];
            this.border = v.border;
            this.borderChar = (char)v.borderCharAscii;
        }
        public void Clear()
        {
            for(int i = 0; i< height - border * 2; i++)
            {
                buffer[i] = new string(Blank, width - border * 2);
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
            Console.Write(new string(borderChar, width - border * 2));
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                for (int j = 0; j < border; j++)
                {
                    Console.Write(borderChar);
                }
            }
            for(int i = 0; i < buffer.Length; i++)
            {
                Console.SetCursorPosition(x + 1, y + i + 1);
                Console.Write(buffer[i] ?? new string(Blank, width - border * 2));
            }
            Console.SetCursorPosition(x + border, y + height - border);
            Console.Write(new string(borderChar, width - border * 2));
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x + width - border, y + i);
                for(int j = 0; j < border; j++)
                {
                    Console.Write(borderChar);
                }
            }
            Console.Write("\n");
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
