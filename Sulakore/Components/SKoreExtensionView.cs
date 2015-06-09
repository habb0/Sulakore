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

using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

using Sulakore.Extensions;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreExtensionView : SKoreListView
    {
        private readonly Dictionary<ExtensionBase, ListViewItem> _items;
        private readonly Dictionary<ListViewItem, ExtensionBase> _extensions;

        public SKoreExtensionView()
        {
            _items = new Dictionary<ExtensionBase, ListViewItem>();
            _extensions = new Dictionary<ListViewItem, ExtensionBase>();
        }

        public void InitializeItemExtension()
        {
            ExtensionBase extension = GetItemExtension();
            if (extension == null) return;

            ((Contractor)extension.Contractor).Initialize(extension);
        }
        public ExtensionBase GetItemExtension() => SelectedItems.Count > 0 && _extensions.ContainsKey(SelectedItems[0]) ?
                _extensions[SelectedItems[0]] : null;

        public void Install(ExtensionBase extension)
        {
            if (extension == null) return;

            ListViewItem item = FocusAdd(extension.Name,
                extension.Author, extension.Version.ToString());

            _items[extension] = item;
            _extensions[item] = extension;

        }
        protected override void RemoveItem(ListViewItem listViewItem)
        {
            ExtensionBase extension = GetItemExtension();
            if (extension == null) return;
            ((Contractor)extension.Contractor).Uninstall(extension);

            _items.Remove(extension);
            _extensions.Remove(listViewItem);

            base.RemoveItem(listViewItem);
        }
    }
}