using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Alta.Tools
{
	public class TouchBoard {

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool PostMessage(int hWnd, uint Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(String sClassName, String sAppName);

		public static Process Open()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			return Process.Start(startInfo);
		}
		public static void Close(Process p)
		{
			uint WM_SYSCOMMAND = 274;
			uint SC_CLOSE = 61536;
            IntPtr KeyboardWnd = p.Handle;  //FindWindow("IPTip_Main_Window", null);
			PostMessage(KeyboardWnd.ToInt32(), WM_SYSCOMMAND, (int)SC_CLOSE, 0);
		}
	}
}