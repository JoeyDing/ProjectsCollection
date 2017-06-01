﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems.WindowItems;

namespace SBSAutomation.Core
{
    public static class WindowExtensions
    {
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;

        // see https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545(v=vs.85).aspx
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hwndAfter, int x, int y, int width, int height, int flags);

        public static void Resize(this Window window, int width, int height)
        {
            SetWindowPos(window, 0, 0, width, height, SWP_NOMOVE | SWP_NOZORDER);
        }

        public static void Move(this Window window, int x, int y)
        {
            SetWindowPos(window, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
        }

        public static void Move(this Window window, int x, int y, int width, int height)
        {
            SetWindowPos(window, x, y, width, height, SWP_NOZORDER);
        }

        private static void SetWindowPos(this Window window, int x, int y, int width, int height, int flags)
        {
            var handle = new IntPtr(window.AutomationElement.Current.NativeWindowHandle);
            SetWindowPos(handle, IntPtr.Zero, x, y, width, height, flags);
        }

    }
}
