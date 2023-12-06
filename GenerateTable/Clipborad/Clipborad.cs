using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GenerateTable
{
    /// <summary>
    /// 剪切板 api 封装
    /// </summary>
    public partial class Clipborad
    {
        [LibraryImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool OpenClipboard(IntPtr hWndNewOwner);

        [LibraryImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool CloseClipboard();

        [LibraryImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool EmptyClipboard();

        [LibraryImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool IsClipboardFormatAvailable(int format);

        [LibraryImport("User32")]
        internal static partial IntPtr GetClipboardData(int uFormat);

        [LibraryImport("User32")]
        internal static partial IntPtr SetClipboardData(int uFormat, IntPtr hMem);

        /// <summary>
        /// 向剪切板写入内容
        /// </summary>
        /// <param name="text"></param>
        public static void SetText(string text)
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                SetText(text);

                return;
            }

            EmptyClipboard();
            SetClipboardData(ClipboardFormat.CF_UNICODETEXT, Marshal.StringToHGlobalUni(text));
            CloseClipboard();
        }

        /// <summary>
        /// 从剪切板获取内容
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string? GetText(int format)
        {
            string? value = string.Empty;
            OpenClipboard(IntPtr.Zero);
            if (IsClipboardFormatAvailable(format))
            {
                IntPtr ptr = Clipborad.GetClipboardData(format);
                if (ptr != IntPtr.Zero)
                {
                    value = Marshal.PtrToStringUni(ptr);
                }
            }
            CloseClipboard();

            return value;
        }
    }
}
