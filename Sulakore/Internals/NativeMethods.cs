/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel related desktop applications.
    Copyright (C) 2015 ArachisH

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

    See License.txt in the project root for license information.
*/

using System;
using System.Text;
using System.Runtime.InteropServices;

using Microsoft.Win32;

namespace Sulakore
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(IntPtr hwnd, uint msg, int wparam, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        private static bool settingsReturn, refreshReturn;
        private static readonly RegistryKey ProxyRegistry;

        static NativeMethods()
        {
            ProxyRegistry = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings", true);
        }

        public static void DisableProxy()
        {
            if (ProxyRegistry.GetValue("ProxyServer") != null)
                ProxyRegistry.DeleteValue("ProxyServer");

            ProxyRegistry.SetValue("ProxyEnable", 0);
            ProxyRegistry.SetValue("ProxyOverride", "<-loopback>");
            RefreshIESettings();
        }
        public static void EnableProxy(int httpPort)
        {
            const string singlePort = "http=127.0.0.1:{0}";
            EnableProxy(string.Format(singlePort, httpPort));
        }
        public static void EnableProxy(int httpPort, int httpsPort)
        {
            const string multiplePorts = "http=127.0.0.1:{0};https=127.0.0.1:{1}";
            EnableProxy(string.Format(multiplePorts, httpPort, httpsPort));
        }

        private static void RefreshIESettings()
        {
            settingsReturn = InternetSetOption(IntPtr.Zero, 39, IntPtr.Zero, 0);
            refreshReturn = InternetSetOption(IntPtr.Zero, 37, IntPtr.Zero, 0);
        }
        private static void EnableProxy(string proxySettings)
        {
            ProxyRegistry.SetValue("ProxyServer", proxySettings);

            ProxyRegistry.SetValue("ProxyEnable", 1);
            ProxyRegistry.SetValue("ProxyOverride", "<-loopback>;<local>");
            RefreshIESettings();
        }
    }
}