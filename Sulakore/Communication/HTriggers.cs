/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel desktop applications.
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

using Sulakore.Habbo.Headers;
using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    public class HTriggers : IDisposable
    {
        private readonly Stack<HMessage> _outPrevious, _inPrevious;
        private readonly IDictionary<ushort, Action<HMessage>> _outLocked, _inLocked, _inCallbacks, _outCallbacks;

        #region Incoming Game Event Handlers
        public event EventHandler<FurnitureLoadEventArgs> FurnitureLoad;
        protected virtual void OnFurnitureLoad(FurnitureLoadEventArgs e)
        {
            EventHandler<FurnitureLoadEventArgs> handler = FurnitureLoad;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnFurnitureLoad(HMessage packet)
        {
            OnFurnitureLoad(new FurnitureLoadEventArgs(packet));
        }

        public event EventHandler<FurnitureDropEventArgs> FurnitureDrop;
        protected virtual void OnFurnitureDrop(FurnitureDropEventArgs e)
        {
            EventHandler<FurnitureDropEventArgs> handler = FurnitureDrop;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnFurnitureDrop(HMessage packet)
        {
            OnFurnitureDrop(new FurnitureDropEventArgs(packet));
        }

        public event EventHandler<FurnitureMoveEventArgs> FurnitureMove;
        protected virtual void OnFurnitureMove(FurnitureMoveEventArgs e)
        {
            EventHandler<FurnitureMoveEventArgs> handler = FurnitureMove;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnFurnitureMove(HMessage packet)
        {
            OnFurnitureMove(new FurnitureMoveEventArgs(packet));
        }

        public event EventHandler<SentienceLoadEventArgs> SentienceLoad;
        protected virtual void OnSentienceLoad(SentienceLoadEventArgs e)
        {
            EventHandler<SentienceLoadEventArgs> handler = SentienceLoad;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnSentienceLoad(HMessage packet)
        {
            OnSentienceLoad(new SentienceLoadEventArgs(packet));
        }

        public event EventHandler<SentienceActionEventArgs> SentienceAction;
        protected virtual void OnSentienceAction(SentienceActionEventArgs e)
        {
            EventHandler<SentienceActionEventArgs> handler = SentienceAction;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnSentienceAction(HMessage packet)
        {
            OnSentienceAction(new SentienceActionEventArgs(packet));
        }

        public event EventHandler<PlayerKickHostEventArgs> PlayerKickHost;
        protected virtual void OnPlayerKickHost(PlayerKickHostEventArgs e)
        {
            EventHandler<PlayerKickHostEventArgs> handler = PlayerKickHost;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnPlayerKickHost(HMessage packet)
        {
            OnPlayerKickHost(new PlayerKickHostEventArgs(packet));
        }

        public event EventHandler<PlayerUpdateEventArgs> PlayerUpdate;
        protected virtual void OnPlayerUpdate(PlayerUpdateEventArgs e)
        {
            EventHandler<PlayerUpdateEventArgs> handler = PlayerUpdate;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnPlayerUpdate(HMessage packet)
        {
            OnPlayerUpdate(new PlayerUpdateEventArgs(packet));
        }

        public event EventHandler<PlayerDanceEventArgs> PlayerDance;
        protected virtual void OnPlayerDance(PlayerDanceEventArgs e)
        {
            EventHandler<PlayerDanceEventArgs> handler = PlayerDance;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnPlayerDance(HMessage packet)
        {
            OnPlayerDance(new PlayerDanceEventArgs(packet));
        }

        public event EventHandler<PlayerGestureEventArgs> PlayerGesture;
        protected virtual void OnPlayerGesture(PlayerGestureEventArgs e)
        {
            EventHandler<PlayerGestureEventArgs> handler = PlayerGesture;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnPlayerGesture(HMessage packet)
        {
            OnPlayerGesture(new PlayerGestureEventArgs(packet));
        }
        #endregion
        #region Outgoing Game Event Handlers
        public event EventHandler<HostBanPlayerEventArgs> HostBanPlayer;
        protected virtual void OnHostBanPlayer(HostBanPlayerEventArgs e)
        {
            EventHandler<HostBanPlayerEventArgs> handler = HostBanPlayer;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostBanPlayer(HMessage packet)
        {
            OnHostBanPlayer(new HostBanPlayerEventArgs(packet));
        }

        public event EventHandler<HostUpdateClothesEventArgs> HostUpdateClothes;
        protected virtual void OnHostUpdateClothes(HostUpdateClothesEventArgs e)
        {
            EventHandler<HostUpdateClothesEventArgs> handler = HostUpdateClothes;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostUpdateClothes(HMessage packet)
        {
            OnHostUpdateClothes(new HostUpdateClothesEventArgs(packet));
        }

        public event EventHandler<HostUpdateMottoEventArgs> HostUpdateMotto;
        protected virtual void OnHostUpdateMotto(HostUpdateMottoEventArgs e)
        {
            EventHandler<HostUpdateMottoEventArgs> handler = HostUpdateMotto;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostUpdateMotto(HMessage packet)
        {
            OnHostUpdateMotto(new HostUpdateMottoEventArgs(packet));
        }

        public event EventHandler<HostUpdateStanceEventArgs> HostUpdateStance;
        protected virtual void OnHostUpdateStance(HostUpdateStanceEventArgs e)
        {
            EventHandler<HostUpdateStanceEventArgs> handler = HostUpdateStance;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostUpdateStance(HMessage packet)
        {
            OnHostUpdateStance(new HostUpdateStanceEventArgs(packet));
        }

        public event EventHandler<HostClickPlayerEventArgs> HostClickPlayer;
        protected virtual void OnHostClickPlayer(HostClickPlayerEventArgs e)
        {
            EventHandler<HostClickPlayerEventArgs> handler = HostClickPlayer;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostClickPlayer(HMessage packet)
        {
            OnHostClickPlayer(new HostClickPlayerEventArgs(packet));
        }

        public event EventHandler<HostDanceEventArgs> HostDance;
        protected virtual void OnHostDance(HostDanceEventArgs e)
        {
            EventHandler<HostDanceEventArgs> handler = HostDance;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostDance(HMessage packet)
        {
            OnHostDance(new HostDanceEventArgs(packet));
        }

        public event EventHandler<HostGestureEventArgs> HostGesture;
        protected virtual void OnHostGesture(HostGestureEventArgs e)
        {
            EventHandler<HostGestureEventArgs> handler = HostGesture;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostGesture(HMessage packet)
        {
            OnHostGesture(new HostGestureEventArgs(packet));
        }

        public event EventHandler<HostKickPlayerEventArgs> HostKickPlayer;
        protected virtual void OnHostKickPlayer(HostKickPlayerEventArgs e)
        {
            EventHandler<HostKickPlayerEventArgs> handler = HostKickPlayer;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostKickPlayer(HMessage packet)
        {
            OnHostKickPlayer(new HostKickPlayerEventArgs(packet));
        }

        public event EventHandler<HostMoveFurnitureEventArgs> HostMoveFurniture;
        protected virtual void OnHostMoveFurniture(HostMoveFurnitureEventArgs e)
        {
            EventHandler<HostMoveFurnitureEventArgs> handler = HostMoveFurniture;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostMoveFurniture(HMessage packet)
        {
            OnHostMoveFurniture(new HostMoveFurnitureEventArgs(packet));
        }

        public event EventHandler<HostMutePlayerEventArgs> HostMutePlayer;
        protected virtual void OnHostMutePlayer(HostMutePlayerEventArgs e)
        {
            EventHandler<HostMutePlayerEventArgs> handler = HostMutePlayer;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostMutePlayer(HMessage packet)
        {
            OnHostMutePlayer(new HostMutePlayerEventArgs(packet));
        }

        public event EventHandler<HostRaiseSignEventArgs> HostRaiseSign;
        protected virtual void OnHostRaiseSign(HostRaiseSignEventArgs e)
        {
            EventHandler<HostRaiseSignEventArgs> handler = HostRaiseSign;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostRaiseSign(HMessage packet)
        {
            OnHostRaiseSign(new HostRaiseSignEventArgs(packet));
        }

        public event EventHandler<HostExitRoomEventArgs> HostExitRoom;
        protected virtual void OnHostExitRoom(HostExitRoomEventArgs e)
        {
            EventHandler<HostExitRoomEventArgs> handler = HostExitRoom;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostExitRoom(HMessage packet)
        {
            OnHostExitRoom(new HostExitRoomEventArgs(packet));
        }

        public event EventHandler<HostNavigateRoomEventArgs> HostNavigateRoom;
        protected virtual void OnHostNavigateRoom(HostNavigateRoomEventArgs e)
        {
            EventHandler<HostNavigateRoomEventArgs> handler = HostNavigateRoom;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostNavigateRoom(HMessage packet)
        {
            OnHostNavigateRoom(new HostNavigateRoomEventArgs(packet));
        }

        public event EventHandler<HostSayEventArgs> HostSay;
        protected virtual void OnHostSay(HostSayEventArgs e)
        {
            EventHandler<HostSayEventArgs> handler = HostSay;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostSay(HMessage packet)
        {
            OnHostSay(new HostSayEventArgs(packet));
        }

        public event EventHandler<HostShoutEventArgs> HostShout;
        protected virtual void OnHostShout(HostShoutEventArgs e)
        {
            EventHandler<HostShoutEventArgs> handler = HostShout;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostShout(HMessage packet)
        {
            OnHostShout(new HostShoutEventArgs(packet));
        }

        public event EventHandler<HostTradeEventArgs> HostTradePlayer;
        protected virtual void OnHostTradePlayer(HostTradeEventArgs e)
        {
            EventHandler<HostTradeEventArgs> handler = HostTradePlayer;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostTradePlayer(HMessage packet)
        {
            OnHostTradePlayer(new HostTradeEventArgs(packet));
        }

        public event EventHandler<HostWalkEventArgs> HostWalk;
        protected virtual void OnHostWalk(HostWalkEventArgs e)
        {
            EventHandler<HostWalkEventArgs> handler = HostWalk;
            if (handler != null) handler(this, e);
        }
        protected void RaiseOnHostWalk(HMessage packet)
        {
            OnHostWalk(new HostWalkEventArgs(packet));
        }
        #endregion
        
        public bool IsDetecting { get; set; }

        public static bool UpdateHeaders { get; set; }

        public HTriggers()
            : this(true)
        { }
        public HTriggers(bool isDetecting)
        {
            IsDetecting = isDetecting;

            _inPrevious = new Stack<HMessage>();
            _outPrevious = new Stack<HMessage>();

            _inLocked = new Dictionary<ushort, Action<HMessage>>();
            _outLocked = new Dictionary<ushort, Action<HMessage>>();

            _inCallbacks = new Dictionary<ushort, Action<HMessage>>();
            _outCallbacks = new Dictionary<ushort, Action<HMessage>>();
        }

        public void InDetach()
        {
            _inCallbacks.Clear();
        }
        public void InDetach(ushort header)
        {
            if (_inCallbacks.ContainsKey(header))
                _inCallbacks.Remove(header);
        }
        public void InAttach(ushort header, Action<HMessage> callback)
        {
            _inCallbacks[header] = callback;
        }

        public void OutDetach()
        {
            _outCallbacks.Clear();
        }
        public void OutDetach(ushort header)
        {
            if (_outCallbacks.ContainsKey(header))
                _outCallbacks.Remove(header);
        }
        public void OutAttach(ushort header, Action<HMessage> callback)
        {
            _outCallbacks[header] = callback;
        }

        public void ProcessOutgoing(byte[] data)
        {
            var current = new HMessage(data, HDestination.Server);
            ProcessOutgoing(current);
        }
        public void ProcessOutgoing(HMessage current)
        {
            if (current == null || current.IsCorrupted) return;
            bool ignoreCurrent = false;

            try
            {
                if (_outCallbacks.ContainsKey(current.Header))
                    _outCallbacks[current.Header](current);

                if (_outPrevious.Count > 0 && IsDetecting)
                {
                    HMessage previous = null;

                    if (_outPrevious.Count > 0)
                        previous = _outPrevious.Pop();

                    if (_outLocked.ContainsKey(current.Header))
                        _outLocked[current.Header](current);
                    else if (previous != null && (IsDetecting && !_outLocked.ContainsKey(previous.Header)))
                        ignoreCurrent = ProcessOutgoing(current, previous);
                }
            }
            finally
            {
                if (!ignoreCurrent)
                {
                    current.Position = 0;

                    if (IsDetecting)
                        _outPrevious.Push(current);
                }
            }

        }
        protected virtual bool ProcessOutgoing(HMessage current, HMessage previous)
        {
            if (current.Length == 6)
            {
                // Range: 6
                if (TryProcessAvatarMenuClick(current, previous)) return true;
                if (TryProcessHostExitRoom(current, previous)) return true;
            }
            else if (current.Length >= 36 && current.Length <= 50)
            {
                //Range: 36 - 50
                if (TryProcessHostRaiseSign(current, previous)) return true;
                if (TryProcessHostNavigateRoom(current, previous)) return true;
            }
            return false;
        }

        public void ProcessIncoming(byte[] data)
        {
            var current = new HMessage(data, HDestination.Client);
            ProcessIncoming(current);
        }
        public void ProcessIncoming(HMessage current)
        {
            if (current == null || current.IsCorrupted) return;
            bool ignoreCurrent = false;

            try
            {
                if (_inCallbacks.ContainsKey(current.Header))
                    _inCallbacks[current.Header](current);

                if (_inPrevious.Count > 0 && IsDetecting)
                {
                    HMessage previous = _inPrevious.Pop();

                    if (_inLocked.ContainsKey(current.Header))
                        _inLocked[current.Header](current);
                    else if (previous != null && (IsDetecting && !_inLocked.ContainsKey(previous.Header)))
                        ignoreCurrent = ProcessIncoming(current, previous);
                }
            }
            finally
            {
                if (!ignoreCurrent)
                {
                    current.Position = 0;

                    if (IsDetecting)
                        _inPrevious.Push(current);
                }
            }
        }
        protected virtual bool ProcessIncoming(HMessage current, HMessage previous)
        {
            if (current.Length == 6)
            {
                // Range: 6
                if (TryProcessPlayerKickHost(current, previous)) return true;
            }
            return false;
        }

        protected void InLockHeader(ushort header, Action<HMessage> eventRaiser)
        {
            _inLocked[header] = eventRaiser;
        }
        protected void OutLockHeader(ushort header, Action<HMessage> eventRaiser)
        {
            _outLocked[header] = eventRaiser;
        }

        private bool TryProcessHostExitRoom(HMessage current, HMessage previous)
        {
            if (previous.Length != 2 || current.ReadInteger(0) != -1) return false;

            if (UpdateHeaders)
                Outgoing.Global.ExitRoom = previous.Header;

            OutLockHeader(previous.Header, RaiseOnHostExitRoom);
            RaiseOnHostExitRoom(previous);
            return true;
        }
        private bool TryProcessHostRaiseSign(HMessage current, HMessage previous)
        {
            bool isHostRaiseSign = false;
            if (current.CanRead<string>(22) && current.ReadString(22) == "sign")
            {
                if (UpdateHeaders)
                    Outgoing.Global.RaiseSign = previous.Header;

                OutLockHeader(previous.Header, RaiseOnHostRaiseSign);
                RaiseOnHostRaiseSign(previous);
                isHostRaiseSign = true;
            }
            return isHostRaiseSign;
        }
        private bool TryProcessPlayerKickHost(HMessage current, HMessage previous)
        {
            bool isPlayerKickHost = (current.ReadInteger(0) == 4008);
            if (isPlayerKickHost)
            {
                if (UpdateHeaders)
                    Incoming.PlayerKickHost = current.Header;

                InLockHeader(current.Header, RaiseOnPlayerKickHost);
                RaiseOnPlayerKickHost(current);
            }
            return isPlayerKickHost;
        }
        private bool TryProcessAvatarMenuClick(HMessage current, HMessage previous)
        {
            if (!previous.CanRead<string>(22)) return false;
            switch (previous.ReadString(22))
            {
                case "sit":
                case "stand":
                {
                    if (UpdateHeaders)
                        Outgoing.Global.UpdateStance = current.Header;

                    OutLockHeader(current.Header, RaiseOnHostUpdateStance);
                    RaiseOnHostUpdateStance(current);
                    return true;
                }

                case "dance_stop":
                case "dance_start":
                {
                    if (UpdateHeaders)
                        Outgoing.Global.Dance = current.Header;

                    OutLockHeader(current.Header, RaiseOnHostDance);
                    RaiseOnHostDance(current);
                    return true;
                }

                case "wave":
                case "idle":
                case "laugh":
                case "blow_kiss":
                {
                    if (UpdateHeaders)
                        Outgoing.Global.Gesture = current.Header;

                    OutLockHeader(current.Header, RaiseOnHostGesture);
                    RaiseOnHostGesture(current);
                    return true;
                }
            }
            return false;
        }
        private bool TryProcessHostNavigateRoom(HMessage current, HMessage previous)
        {
            if (previous.Length >= 12 && current.CanRead<string>(0)
                && current.ReadString() == "Navigation")
            {
                current.ReadString();
                if (current.ReadString() != "go.official") return false;

                if (previous.ReadInteger(0).ToString() == current.ReadString())
                {
                    if (UpdateHeaders)
                        Outgoing.Global.NavigateRoom = previous.Header;

                    OutLockHeader(previous.Header, RaiseOnHostExitRoom);
                    RaiseOnHostNavigateRoom(previous);
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inPrevious.Clear();
                _outPrevious.Clear();

                _inLocked.Clear();
                _outLocked.Clear();

                _inCallbacks.Clear();
                _outCallbacks.Clear();
            }

            HostBanPlayer = null;
            HostUpdateClothes = null;
            HostUpdateMotto = null;
            HostUpdateStance = null;
            HostClickPlayer = null;
            HostDance = null;
            HostGesture = null;
            HostKickPlayer = null;
            HostMoveFurniture = null;
            HostMutePlayer = null;
            HostRaiseSign = null;
            HostExitRoom = null;
            HostNavigateRoom = null;
            HostSay = null;
            HostShout = null;
            HostTradePlayer = null;
            HostWalk = null;

            FurnitureLoad = null;
            SentienceAction = null;
            PlayerUpdate = null;
            PlayerDance = null;
            SentienceLoad = null;
            FurnitureDrop = null;
            PlayerGesture = null;
            PlayerKickHost = null;
            FurnitureMove = null;
        }
    }
}