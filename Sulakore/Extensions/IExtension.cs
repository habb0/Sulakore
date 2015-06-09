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

using Sulakore.Components;

namespace Sulakore.Extensions
{
    public interface IExtension : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the extension is running.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets the name of the extension given by the Author.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Gets the name(s) of the developer(s) that worked on the extension.
        /// </summary>
        string Author { get; }
        /// <summary>
        /// Gets the file location of the extension from which the instance was initialized from.
        /// </summary>
        string Location { get; }

        /// <summary>
        /// Gets the logo of the extension.
        /// </summary>
        Bitmap Logo { get; }
        /// <summary>
        /// Gets the assembly version of the extension.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets the IContractor instance used for communication between the initializer.
        /// </summary>
        IContractor Contractor { get; }
        /// <summary>
        /// Gets or sets the priority of the extension that determines whether a new thread is spawned when handling the flow of data(High), or whether to pull one from the system's thread pool(Normal).
        /// </summary>
        ExtensionPriority Priority { get; set; }

        /// <summary>
        /// Gets the Type found in the extension's project scope that inherits from SKoreExtensionForm.
        /// </summary>
        Type UIContextType { get; }
        /// <summary>
        /// Gets the Form that represents the extension's main GUI.
        /// </summary>
        SKoreForm UIContext { get; }

        void Initialize();

        /// <summary>
        /// Processes all incoming data on a separate thread.
        /// </summary>
        /// <param name="data">The incoming data.</param>
        void DataToClient(byte[] data);
        /// <summary>
        /// Processes all outgoing data on a separate thread.
        /// </summary>
        /// <param name="data">The outgoing data.</param>
        void DataToServer(byte[] data);

        /// <summary>
        /// Attempts to invoke a custom command with arguments.
        /// </summary>
        /// <param name="invoker">The source of the method call.</param>
        /// <param name="command">The command for the invoked to utilize.</param>
        /// <param name="args">The arguments given to be used by the invoked to process the command.</param>
        /// <returns></returns>
        object Invoke(object invoker, string command, params object[] arg);
    }
}