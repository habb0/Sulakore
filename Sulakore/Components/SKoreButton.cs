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
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreButton : Control, IButtonControl
    {
        private bool _isPressed;

        public DialogResult DialogResult { get; set; }

        [Browsable(false)]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Browsable(false)]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        [Browsable(false)]
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set { base.BackgroundImage = value; }
        }

        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set { base.BackgroundImageLayout = value; }
        }

        [SettingsBindable(true)]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; Invalidate(); }
        }

        [DefaultValue(typeof(Size), "100, 22")]
        [Localizable(true)]
        new public Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        private Color _skin = Color.SteelBlue;
        [DefaultValue(typeof(Color), "SteelBlue")]
        public Color Skin
        {
            get { return _skin; }
            set { _skin = value; Invalidate(); }
        }

        public SKoreButton()
        {
            SetStyle((ControlStyles)2050, true);
            DoubleBuffered = true;

            Size = new Size(100, 22);
            BackColor = Color.Transparent;
        }

        public void PerformClick()
        {
            base.OnClick(EventArgs.Empty);
        }
        public void NotifyDefault(bool value)
        { }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Enabled ? Skin : SystemColors.Control);

            using (var pen = new Pen(Color.FromArgb(50, Color.Black)))
                e.Graphics.DrawRectangle(pen, ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

            if (Enabled)
            {
                if (_isPressed)
                {
                    const int height = 10;

                    var r1 = new Rectangle(0, 0, Width, height);
                    using (var linearGradientBrush = new LinearGradientBrush(r1, Color.FromArgb(25, Color.Black), Color.Transparent, 90))
                        e.Graphics.FillRectangle(linearGradientBrush, r1);

                    var r2 = new Rectangle(0, Height - height, Width, height);
                    using (var linearGradientBrush = new LinearGradientBrush(r2, Color.FromArgb(25, Color.Black), Color.Transparent, 270))
                        e.Graphics.FillRectangle(linearGradientBrush, r2);
                }

                using (var solidBrush = new SolidBrush(Color.FromArgb(_isPressed ? 150 : 100, Color.Black)))
                using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString(Text, Font, solidBrush, new Rectangle(1, 1, Width, Height + 1), stringFormat);

                using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString(Text, Font, Brushes.White, new Rectangle(0, 0, Width, Height + 1), stringFormat);
            }
            else
            {
                using (var solidBrush = new SolidBrush(Color.FromArgb(150, Color.Black)))
                using (var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString(Text, Font, solidBrush, new Rectangle(0, 0, Width, Height + 1), stringFormat);
            }
            base.OnPaint(e);
        }

        protected override void OnClick(EventArgs e)
        { }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool isLeft = (e.Button == MouseButtons.Left);
            if (isLeft)
            {
                _isPressed = false;
                Invalidate();
            }
            base.OnMouseUp(e);

            if (ClientRectangle.Contains(e.Location) && isLeft)
                base.OnClick(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isPressed = true;
                Invalidate();
            }
            base.OnMouseDown(e);
        }
    }
}