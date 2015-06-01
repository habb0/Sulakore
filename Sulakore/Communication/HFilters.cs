/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel related desktop applications.
    Copyright (C) 2015 Arachis

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
using System.Collections.Generic;

using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    public class HFilters
    {
        private readonly IList<ushort> _inBlockedHeaders, _outBlockedHeaders;
        private readonly IDictionary<ushort, Predicate<HMessage>> _inBlockConditions, _outBlockConditions;

        private readonly IDictionary<ushort, HMessage> _inReplacements, _outReplacements;
        private readonly IDictionary<ushort, Func<HMessage, HMessage>> _inReplacers, _outReplacers;

        public HFilters()
        {
            _inBlockedHeaders = new List<ushort>();
            _outBlockedHeaders = new List<ushort>();

            _inBlockConditions = new Dictionary<ushort, Predicate<HMessage>>();
            _outBlockConditions = new Dictionary<ushort, Predicate<HMessage>>();

            _inReplacements = new Dictionary<ushort, HMessage>();
            _outReplacements = new Dictionary<ushort, HMessage>();

            _inReplacers = new Dictionary<ushort, Func<HMessage, HMessage>>();
            _outReplacers = new Dictionary<ushort, Func<HMessage, HMessage>>();
        }

        public void InUnblock()
        {
            _inBlockedHeaders.Clear();
            _inBlockConditions.Clear();
        }
        public void OutUnblock()
        {
            _outBlockedHeaders.Clear();
            _outBlockConditions.Clear();
        }
        public void InUnblock(ushort header)
        {
            if (_inBlockedHeaders.Contains(header))
                _inBlockedHeaders.Remove(header);

            if (_inBlockConditions.ContainsKey(header))
                _inBlockConditions.Remove(header);
        }
        public void OutUnblock(ushort header)
        {
            if (_outBlockedHeaders.Contains(header))
                _outBlockedHeaders.Remove(header);

            if (_outBlockConditions.ContainsKey(header))
                _outBlockConditions.Remove(header);
        }

        public void InBlock(ushort header)
        {
            InUnblock(header);
            _inBlockedHeaders.Add(header);
        }
        public void OutBlock(ushort header)
        {
            OutUnblock(header);
            _outBlockedHeaders.Add(header);
        }
        public void InBlock(ushort header, Predicate<HMessage> predicate)
        {
            InUnblock(header);
            _inBlockConditions.Add(header, predicate);
        }
        public void OutBlock(ushort header, Predicate<HMessage> predicate)
        {
            OutUnblock(header);
            _outBlockConditions.Add(header, predicate);
        }

        public void InUnreplace()
        {
            _inReplacers.Clear();
            _inReplacements.Clear();
        }
        public void OutUnreplace()
        {
            _outReplacers.Clear();
            _outReplacements.Clear();
        }
        public void InUnreplace(ushort header)
        {
            if (_inReplacers.ContainsKey(header))
                _inReplacers.Remove(header);

            if (_inReplacements.ContainsKey(header))
                _inReplacements.Remove(header);
        }
        public void OutUnreplace(ushort header)
        {
            if (_outReplacers.ContainsKey(header))
                _outReplacers.Remove(header);

            if (_outReplacements.ContainsKey(header))
                _outReplacements.Remove(header);
        }

        public void InReplace(ushort header, HMessage packet)
        {
            InUnblock(header);
            InUnreplace(header);
            _inReplacements.Add(header, packet);
        }
        public void OutReplace(ushort header, HMessage packet)
        {
            OutUnblock(header);
            OutUnreplace(header);
            _outReplacements.Add(header, packet);
        }
        public void InReplace(ushort header, Func<HMessage, HMessage> replacer)
        {
            InUnblock(header);
            InUnreplace(header);
            _inReplacers.Add(header, replacer);
        }
        public void OutReplace(ushort header, Func<HMessage, HMessage> replacer)
        {
            OutUnblock(header);
            OutUnreplace(header);
            _outReplacers.Add(header, replacer);
        }

        /// <summary>
        /// Returns a value that derermines whether the incoming packet should be blocked, otherwise attempts to apply the filters related to the <see cref="HMessage"/>.
        /// </summary>
        /// <param name="packet">The incoming packet to process.</param>
        /// <param name="repeat">The value type to update with the number of times the packet should be repeated.</param>
        /// <returns>true if the packet should be blocked; otherwise false.</returns>
        public virtual bool InProcessFilters(ref HMessage packet)
        {
            bool passedBlockCheck = false;
            try
            {
                if (_inBlockedHeaders.Contains(packet.Header) || (_inBlockConditions.ContainsKey(packet.Header)
                    && _inBlockConditions[packet.Header](packet)))
                    return true;

                passedBlockCheck = true;

                if (_inReplacements.ContainsKey(packet.Header))
                    packet = _inReplacements[packet.Header];
                else if (_inReplacers.ContainsKey(packet.Header))
                    packet = _inReplacers[packet.Header](packet);
            }
            catch
            {
                if (!passedBlockCheck) InUnblock(packet.Header);
                else InUnreplace(packet.Header);
            }
            return false;
        }
        /// <summary>
        /// Returns a value that determines whether the outgoing packet should be blocked, otherwise attempts to apply the filters related to the <see cref="HMessage"/>.
        /// </summary>
        /// <param name="packet">The outgoing packet to process.</param>
        /// <param name="repeat">The value type to update with the number of times the packet should be repeated.</param>
        /// <returns>true if the packet should be blocked; otherwise false.</returns>
        public virtual bool OutProcessFilters(ref HMessage packet)
        {
            bool passedBlockCheck = false;
            try
            {
                if (_outBlockedHeaders.Contains(packet.Header) || (_outBlockConditions.ContainsKey(packet.Header)
                    && _outBlockConditions[packet.Header](packet)))
                    return true;

                passedBlockCheck = true;

                if (_outReplacements.ContainsKey(packet.Header))
                    packet = _outReplacements[packet.Header];
                else if (_outReplacers.ContainsKey(packet.Header))
                    packet = _outReplacers[packet.Header](packet);
            }
            catch
            {
                if (!passedBlockCheck) OutUnblock(packet.Header);
                else OutUnreplace(packet.Header);
            }
            return false;
        }
    }
}