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
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sulakore.Components;
using Sulakore.Communication;
using Sulakore.Habbo.Web;

namespace Sulakore.Extensions
{
    public class Contractor : IContractor
    {
        private static readonly string _currentAsmName;

        private readonly HConnection _connection;
        private readonly List<IExtension> _extensions, _extensionsRunning;

        private const string EXTENSIONS_DIRECTORY = "Extensions";

        public event EventHandler<InvokedEventArgs> Invoked;
        protected virtual object OnInvoked(InvokedEventArgs e)
        {
            EventHandler<InvokedEventArgs> handler = Invoked;
            if (handler != null) handler(this, e);
            return e.Result;
        }

        public HHotel Hotel { get; }
        public HGameData GameData { get; }
        public ReadOnlyCollection<IExtension> Extensions { get; }
        public ReadOnlyCollection<IExtension> ExtensionsRunning { get; }

        static Contractor()
        {
            _currentAsmName = Assembly.GetExecutingAssembly().FullName;
        }
        public Contractor(HConnection connection, HGameData gameData)
        {
            _connection = connection;

            _extensions = new List<IExtension>();
            Extensions = new ReadOnlyCollection<IExtension>(_extensions);

            _extensionsRunning = new List<IExtension>();
            ExtensionsRunning = new ReadOnlyCollection<IExtension>(_extensionsRunning);

            GameData = gameData;
            //if (connection != null && !string.IsNullOrEmpty(connection.Host))
            //    Hotel = SKore.ToHotel(connection.Host);
        }

        public void ProcessIncoming(byte[] data)
        {
            if (_extensionsRunning.Count < 1) return;

            IExtension[] extensionsRunning = _extensionsRunning.ToArray();
            foreach (IExtension extension in extensionsRunning)
            {
                if (!extension.IsRunning)
                {
                    if (_extensionsRunning.Contains(extension))
                        _extensionsRunning.Remove(extension);

                    continue;
                }

                Task.Factory.StartNew(() => extension.DataToClient(data),
                    (TaskCreationOptions)(extension.Priority == ExtensionPriority.Normal ? 0 : 2));
            }
        }
        public void ProcessOutgoing(byte[] data)
        {
            if (_extensionsRunning.Count < 1) return;

            IExtension[] extensionsRunning = new List<IExtension>(_extensionsRunning).ToArray();
            foreach (IExtension extension in extensionsRunning)
            {
                if (!extension.IsRunning)
                {
                    if (_extensionsRunning.Contains(extension))
                        _extensionsRunning.Remove(extension);

                    continue;
                }

                Task.Factory.StartNew(() => extension.DataToServer(data),
                    (TaskCreationOptions)(extension.Priority == ExtensionPriority.Normal ? 0 : 2));
            }
        }

        public int SendToServer(byte[] data) => 0;// _connection.SendToServer(data);
        public int SendToServer(ushort header, params object[] chunks) => 0;// _connection.SendToServer(header, chunks);

        public int SendToClient(byte[] data) => 0;// _connection.SendToClient(data);
        public int SendToClient(ushort header, params object[] chunks) => 0;// _connection.SendToClient(header, chunks);

        public void Dispose(IExtension extension)
        {
            extension.Dispose();

            if (!extension.IsRunning && _extensionsRunning.Contains(extension))
                _extensionsRunning.Remove(extension);
        }
        public void Initialize(IExtension extension)
        {
            extension.Initialize();

            if (extension.IsRunning && !_extensionsRunning.Contains(extension))
                _extensionsRunning.Add(extension);
        }

        public ExtensionBase Install(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path) || !path.EndsWith(".dll"))
                return null;

            ExtensionBase extension = null;
            if (!Directory.Exists(EXTENSIONS_DIRECTORY))
                Directory.CreateDirectory(EXTENSIONS_DIRECTORY);

            string extensionPath = path;
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, EXTENSIONS_DIRECTORY, Path.GetFileName(path))))
            {
                string extensionId = Guid.NewGuid().ToString();
                string extensionName = Path.GetFileNameWithoutExtension(path);
                extensionPath = Path.Combine(Environment.CurrentDirectory, EXTENSIONS_DIRECTORY, string.Format("{0}({1}).dll", extensionName, extensionId));
                File.Copy(path, extensionPath);
            }

            byte[] extensionData = File.ReadAllBytes(extensionPath);
            Assembly extensionAssembly = Assembly.Load(extensionData);

            Type extensionFormType = null;
            Type[] extensionTypes = extensionAssembly.GetTypes();
            foreach (Type extensionType in extensionTypes)
            {
                if (extensionType.IsInterface || extensionType.IsAbstract) continue;
                if (extensionFormType == null && extensionType.BaseType == typeof(SKoreForm))
                {
                    if (extension == null) extensionFormType = extensionType;
                    else extension.UIContextType = extensionFormType = extensionType;
                }

                if (extensionType.BaseType == typeof(ExtensionBase))
                {
                    extension = (ExtensionBase)Activator.CreateInstance(extensionType);

                    extension.Contractor = this;
                    extension.Location = extensionPath;
                    extension.Triggers = new HTriggers(true);
                    extension.UIContextType = extensionFormType;
                    extension.Version = new Version(FileVersionInfo.GetVersionInfo(extensionPath).FileVersion);
                }
            }

            if (extension != null)
            {
                _extensions.Add(extension);

                object extensionResponse = extension.Invoke(this, "SHOULD_INITIALIZE");
                if (extensionResponse != null && (bool)extensionResponse)
                    Initialize(extension);
            }
            else File.Delete(extensionPath);

            return extension;
        }
        public void Uninstall(IExtension extension)
        {
            if (File.Exists(extension.Location))
                File.Delete(extension.Location);

            Dispose(extension);

            if (_extensions.Contains(extension))
                _extensions.Remove(extension);
        }

        public object Invoke(object invoker, string command, params object[] args) => OnInvoked(new InvokedEventArgs(invoker, command, args));
    }
}