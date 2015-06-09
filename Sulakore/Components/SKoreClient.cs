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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using Sulakore.Habbo;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreClient : WebBrowser
    {
        private IntPtr _ieHandle;
        private IntPtr IEHandle
        {
            get
            {
                if (_ieHandle != IntPtr.Zero) return _ieHandle;

                var bcName = new StringBuilder(100);
                IntPtr handle = Handle;
                do NativeMethods.GetClassName((handle = NativeMethods.GetWindow(handle, 5)), bcName, bcName.MaxCapacity);
                while (bcName.ToString() != "Internet Explorer_Server");

                return (_ieHandle = handle);
            }
        }

        public SKoreClient()
        {
            ScriptErrorsSuppressed = true;
            ScrollBarsEnabled = false;
        }

        new public void Enter()
        {
            NativeMethods.PostMessage(IEHandle, 256u, 13, IntPtr.Zero);
        }
        new public void Click(int x, int y)
        {
            NativeMethods.SendMessage(IEHandle, 0x201, IntPtr.Zero, (IntPtr)((y << 16) | x));
            NativeMethods.SendMessage(IEHandle, 0x202, IntPtr.Zero, (IntPtr)((y << 16) | x));
        }
        new public void Click(Point coordinate)
        {
            Click(coordinate.X, coordinate.Y);
        }

        public void Say(string message)
        {
            Speak(message, false);
        }
        public void Shout(string message)
        {
            Speak(message, true);
        }
        public void Speak(string message, bool shout)
        {
            if (string.IsNullOrEmpty(message)) return;

            Enter();
            if (shout) message = ":shout " + message;
            foreach (char c in message) NativeMethods.PostMessage(IEHandle, 0x102, c, IntPtr.Zero);
            Enter();
        }

        public void Sign(HSign sign)
        {
            Say(":sign " + sign.Juice());
        }
        public void Stance(HStance stance)
        {
            Say(":" + stance.ToString());
        }
        public void Gesture(HGesture gesture)
        {
            switch (gesture)
            {
                case HGesture.Wave: Say("o/"); break;
                case HGesture.Idle: Say(":idle"); break;
                case HGesture.ThumbsUp: Say("_b"); break;
                case HGesture.BlowKiss: Say(":kiss"); break;
                case HGesture.Laugh: Say(":whisper  :D"); break;
            }
        }
    }
}