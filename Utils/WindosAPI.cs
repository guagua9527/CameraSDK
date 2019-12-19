using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CameraSDK.Utils
{
    public class WindosAPI
    {
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32.dll")]
        static public extern IntPtr DeleteDC(IntPtr hDC);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left; //最左坐标

            public int Top; //最上坐标

            public int Right; //最右坐标

            public int Bottom; //最下坐标
        }

        public static void GetControlSize(IntPtr hwnd, out int width, out int height)
        {
            RECT rect = new RECT();

            if (!GetWindowRect(hwnd, ref rect))
            {
                width = 0;
                height = 0;
                return;
            }
            //获取显示控件的宽高
            width = rect.Right - rect.Left;
            height = rect.Bottom - rect.Top;
        }
    }
}
