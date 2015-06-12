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
using System.ComponentModel;
using System.Threading.Tasks;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    /// <summary>
    /// Represents an intercepted message that will be returned to the caller with blocking/replacing information.
    /// </summary>
    public class InterceptedEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets the current count/step/order from which this data was intercepted.
        /// </summary>
        public int Step { get; }
        /// <summary>
        /// Gets a value that determines whether the <see cref="InterceptedEventArgs"/> can be turned into a non-blocking operation by calling <see cref="ContinueRead"/>.
        /// </summary>
        public bool IsAsyncCapable { get; }
        /// <summary>
        /// Gets the intercepted <see cref="HMessage"/> that allows for read/write operations.
        /// </summary>
        public HMessage Packet { get; set; }
        /// <summary>
        /// Gets the <see cref="Func{TResult}"/> of type <see cref="Task"/> that will be invoked when <see cref="ContinueRead"/> is called.
        /// </summary>
        internal Func<Task> Continuation { get; set; }
        /// <summary>
        /// Gets a value that determines whether <see cref="ContinueRead"/> was called by the receiver.
        /// </summary>
        public bool WasContinued { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptedEventArgs"/> class.
        /// </summary>
        /// <param name="packet">The intercepted data to read/write from.</param>
        public InterceptedEventArgs(HMessage packet)
            : this(null, -1, packet)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptedEventArgs"/> class.
        /// </summary>
        /// <param name="step">The current count/step/order from which this data was intercepted.</param>
        /// <param name="packet">The intercepted data to read/write from.</param>
        public InterceptedEventArgs(int step, HMessage packet)
            : this(null, step, packet)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptedEventArgs"/> class.
        /// </summary>
        /// <param name="step">The current count/step/order from which this data was intercepted.</param>
        /// <param name="data">An array of type <see cref="byte"/> that contains the data to convert to an <see cref="HMessage"/>.</param>
        /// <param name="destination">The destination type that will help initialize the <see cref="HMessage"/>.</param>
        public InterceptedEventArgs(int step, byte[] data, HDestination destination)
            : this(null, step, new HMessage(data, destination))
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptedEventArgs"/> class.
        /// </summary>
        /// <param name="continuation">The <see cref="Func{TResult}"/> of type <see cref="Task"/> that will be invoked when <see cref="ContinueRead"/> is called.</param>
        /// <param name="step">The current count/step/order from which this data was intercepted.</param>
        /// <param name="packet">The intercepted data to read/write from.</param>
        public InterceptedEventArgs(Func<Task> continuation, int step, HMessage packet)
        {
            Continuation = continuation;
            IsAsyncCapable = (Continuation != null);

            Step = step;
            Packet = packet;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptedEventArgs"/> class.
        /// </summary>
        /// <param name="continuation">The <see cref="Func{TResult}"/> of type <see cref="Task"/> that will be invoked when <see cref="ContinueRead"/> is called.</param>
        /// <param name="step">The current count/step/order from which this data was intercepted.</param>
        /// <param name="data">An array of type <see cref="byte"/> that contains the data to convert to an <see cref="HMessage"/>.</param>
        /// <param name="destination">The destination type that will help initialize the <see cref="HMessage"/>.</param>
        public InterceptedEventArgs(Func<Task> continuation, int step, byte[] data, HDestination destination)
            : this(continuation, step, new HMessage(data, destination))
        { }

        /// <summary>
        /// Invokes the <see cref="Func{TResult}"/> of type <see cref="Task"/> if <see cref="IsAsyncCapable"/> is true.
        /// </summary>
        public void ContinueRead()
        {
            if (!IsAsyncCapable || WasContinued) return;
            else WasContinued = true;

            Continuation();
        }
    }
}