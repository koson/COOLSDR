// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: https://pvs-studio.com
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoolComponentsTest
{
    public static class TextBoxExtensions
    {
        public static void SetInnerMargins(this TextBoxBase textBox, int left, int top, int right, int bottom)
        {
            var rect = textBox.GetFormattingRect();

            var newRect = new Rectangle(left, top, rect.Width - left - right, rect.Height - top - bottom);
            textBox.SetFormattingRect(newRect);
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct RECT
        {
            public readonly int Left;
            public readonly int Top;
            public readonly int Right;
            public readonly int Bottom;

            private RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }
        }

        [DllImport(@"User32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessageRefRect(IntPtr hWnd, uint msg, int wParam, ref RECT rect);

        [DllImport(@"user32.dll", EntryPoint = @"SendMessage", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, ref Rectangle lParam);

        private const int EM_GETRECT = 0xB2;
        private const int EM_SETRECT = 0xB3;

        public static int SetFormattingRect(this TextBoxBase textbox, Rectangle rect)
        {
            Debug.Assert(textbox.Multiline); // you cannot set the height on a single line textbox
            var rc = new RECT(rect);
            return SendMessageRefRect(textbox.Handle, EM_SETRECT, 0, ref rc);
        }

        public static Rectangle GetFormattingRect(this TextBoxBase textbox)
        {
            var rect = new Rectangle();

            // SendMessage here does not return anything meaningful:
            // https://learn.microsoft.com/en-us/windows/win32/controls/em-setrect
            SendMessage(textbox.Handle, EM_GETRECT, (IntPtr)0, ref rect);
            return rect;
        }
    }
}
