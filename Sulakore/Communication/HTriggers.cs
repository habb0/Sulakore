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
using System.Collections.Generic;

using Sulakore.Habbo.Headers;
using Sulakore.Habbo.Protocol;

namespace Sulakore.Communication
{
    public class HTriggers : IDisposable
    {
        #region Incoming Game Event Handlers
        public event EventHandler<FurnitureLoadEventArgs> FurnitureLoad;
        protected void RaiseOnFurnitureLoad(InterceptedEventArgs e)
        {
            if (FurnitureLoad != null)
            {
                var args = new FurnitureLoadEventArgs(e.Continuation, e.Step, e.Packet);
                OnFurnitureLoad(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnFurnitureLoad(FurnitureLoadEventArgs e)
        {
            EventHandler<FurnitureLoadEventArgs> handler = FurnitureLoad;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<FurnitureDropEventArgs> FurnitureDrop;
        protected void RaiseOnFurnitureDrop(InterceptedEventArgs e)
        {
            if (FurnitureDrop != null)
            {
                var args = new FurnitureDropEventArgs(e.Continuation, e.Step, e.Packet);
                OnFurnitureDrop(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnFurnitureDrop(FurnitureDropEventArgs e)
        {
            EventHandler<FurnitureDropEventArgs> handler = FurnitureDrop;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<FurnitureMoveEventArgs> FurnitureMove;
        protected void RaiseOnFurnitureMove(InterceptedEventArgs e)
        {
            if (FurnitureMove != null)
            {
                var args = new FurnitureMoveEventArgs(e.Continuation, e.Step, e.Packet);
                OnFurnitureMove(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnFurnitureMove(FurnitureMoveEventArgs e)
        {
            EventHandler<FurnitureMoveEventArgs> handler = FurnitureMove;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<EntityLoadEventArgs> EntityLoad;
        protected void RaiseOnEntityLoad(InterceptedEventArgs e)
        {
            if (EntityLoad != null)
            {
                var args = new EntityLoadEventArgs(e.Continuation, e.Step, e.Packet);
                OnEntityLoad(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnEntityLoad(EntityLoadEventArgs e)
        {
            EventHandler<EntityLoadEventArgs> handler = EntityLoad;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<EntityActionEventArgs> EntityAction;
        protected void RaiseOnEntityAction(InterceptedEventArgs e)
        {
            if (EntityAction != null)
            {
                var args = new EntityActionEventArgs(e.Continuation, e.Step, e.Packet);
                OnEntityAction(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnEntityAction(EntityActionEventArgs e)
        {
            EventHandler<EntityActionEventArgs> handler = EntityAction;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<PlayerKickHostEventArgs> PlayerKickHost;
        protected void RaiseOnPlayerKickHost(InterceptedEventArgs e)
        {
            if (PlayerKickHost != null)
            {
                var args = new PlayerKickHostEventArgs(e.Continuation, e.Step, e.Packet);
                OnPlayerKickHost(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnPlayerKickHost(PlayerKickHostEventArgs e)
        {
            EventHandler<PlayerKickHostEventArgs> handler = PlayerKickHost;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<PlayerUpdateEventArgs> PlayerUpdate;
        protected void RaiseOnPlayerUpdate(InterceptedEventArgs e)
        {
            if (PlayerUpdate != null)
            {
                var args = new PlayerUpdateEventArgs(e.Continuation, e.Step, e.Packet);
                OnPlayerUpdate(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnPlayerUpdate(PlayerUpdateEventArgs e)
        {
            EventHandler<PlayerUpdateEventArgs> handler = PlayerUpdate;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<PlayerDanceEventArgs> PlayerDance;
        protected void RaiseOnPlayerDance(InterceptedEventArgs e)
        {
            if (PlayerDance != null)
            {
                var args = new PlayerDanceEventArgs(e.Continuation, e.Step, e.Packet);
                OnPlayerDance(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnPlayerDance(PlayerDanceEventArgs e)
        {
            EventHandler<PlayerDanceEventArgs> handler = PlayerDance;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<PlayerGestureEventArgs> PlayerGesture;
        protected void RaiseOnPlayerGesture(InterceptedEventArgs e)
        {
            if (PlayerGesture != null)
            {
                var args = new PlayerGestureEventArgs(e.Continuation, e.Step, e.Packet);
                OnPlayerGesture(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnPlayerGesture(PlayerGestureEventArgs e)
        {
            EventHandler<PlayerGestureEventArgs> handler = PlayerGesture;
            if (handler != null) handler(this, e);
        }
        #endregion
        #region Outgoing Game Event Handlers
        public event EventHandler<HostBanPlayerEventArgs> HostBanPlayer;
        protected void RaiseOnHostBanPlayer(InterceptedEventArgs e)
        {
            if (HostBanPlayer != null)
            {
                var args = new HostBanPlayerEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostBanPlayer(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostBanPlayer(HostBanPlayerEventArgs e)
        {
            EventHandler<HostBanPlayerEventArgs> handler = HostBanPlayer;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostUpdateClothesEventArgs> HostUpdateClothes;
        protected void RaiseOnHostUpdateClothes(InterceptedEventArgs e)
        {
            if (HostUpdateClothes != null)
            {
                var args = new HostUpdateClothesEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostUpdateClothes(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostUpdateClothes(HostUpdateClothesEventArgs e)
        {
            EventHandler<HostUpdateClothesEventArgs> handler = HostUpdateClothes;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostUpdateMottoEventArgs> HostUpdateMotto;
        protected void RaiseOnHostUpdateMotto(InterceptedEventArgs e)
        {
            if (HostUpdateMotto != null)
            {
                var args = new HostUpdateMottoEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostUpdateMotto(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostUpdateMotto(HostUpdateMottoEventArgs e)
        {
            EventHandler<HostUpdateMottoEventArgs> handler = HostUpdateMotto;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostUpdateStanceEventArgs> HostUpdateStance;
        protected void RaiseOnHostUpdateStance(InterceptedEventArgs e)
        {
            if (HostUpdateStance != null)
            {
                var args = new HostUpdateStanceEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostUpdateStance(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostUpdateStance(HostUpdateStanceEventArgs e)
        {
            EventHandler<HostUpdateStanceEventArgs> handler = HostUpdateStance;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostClickPlayerEventArgs> HostClickPlayer;
        protected void RaiseOnHostClickPlayer(InterceptedEventArgs e)
        {
            if (HostClickPlayer != null)
            {
                var args = new HostClickPlayerEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostClickPlayer(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostClickPlayer(HostClickPlayerEventArgs e)
        {
            EventHandler<HostClickPlayerEventArgs> handler = HostClickPlayer;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostDanceEventArgs> HostDance;
        protected void RaiseOnHostDance(InterceptedEventArgs e)
        {
            if (HostDance != null)
            {
                var args = new HostDanceEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostDance(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostDance(HostDanceEventArgs e)
        {
            EventHandler<HostDanceEventArgs> handler = HostDance;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostGestureEventArgs> HostGesture;
        protected void RaiseOnHostGesture(InterceptedEventArgs e)
        {
            if (HostGesture != null)
            {
                var args = new HostGestureEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostGesture(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostGesture(HostGestureEventArgs e)
        {
            EventHandler<HostGestureEventArgs> handler = HostGesture;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostKickPlayerEventArgs> HostKickPlayer;
        protected void RaiseOnHostKickPlayer(InterceptedEventArgs e)
        {
            if (HostKickPlayer != null)
            {
                var args = new HostKickPlayerEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostKickPlayer(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostKickPlayer(HostKickPlayerEventArgs e)
        {
            EventHandler<HostKickPlayerEventArgs> handler = HostKickPlayer;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostMoveFurnitureEventArgs> HostMoveFurniture;
        protected void RaiseOnHostMoveFurniture(InterceptedEventArgs e)
        {
            if (HostMoveFurniture != null)
            {
                var args = new HostMoveFurnitureEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostMoveFurniture(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostMoveFurniture(HostMoveFurnitureEventArgs e)
        {
            EventHandler<HostMoveFurnitureEventArgs> handler = HostMoveFurniture;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostMutePlayerEventArgs> HostMutePlayer;
        protected void RaiseOnHostMutePlayer(InterceptedEventArgs e)
        {
            if (HostMutePlayer != null)
            {
                var args = new HostMutePlayerEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostMutePlayer(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostMutePlayer(HostMutePlayerEventArgs e)
        {
            EventHandler<HostMutePlayerEventArgs> handler = HostMutePlayer;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostRaiseSignEventArgs> HostRaiseSign;
        protected void RaiseOnHostRaiseSign(InterceptedEventArgs e)
        {
            if (HostRaiseSign != null)
            {
                var args = new HostRaiseSignEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostRaiseSign(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostRaiseSign(HostRaiseSignEventArgs e)
        {
            EventHandler<HostRaiseSignEventArgs> handler = HostRaiseSign;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostExitRoomEventArgs> HostExitRoom;
        protected void RaiseOnHostExitRoom(InterceptedEventArgs e)
        {
            if (HostExitRoom != null)
            {
                var args = new HostExitRoomEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostExitRoom(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostExitRoom(HostExitRoomEventArgs e)
        {
            EventHandler<HostExitRoomEventArgs> handler = HostExitRoom;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostNavigateRoomEventArgs> HostNavigateRoom;
        protected void RaiseOnHostNavigateRoom(InterceptedEventArgs e)
        {
            if (HostNavigateRoom != null)
            {
                var args = new HostNavigateRoomEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostNavigateRoom(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostNavigateRoom(HostNavigateRoomEventArgs e)
        {
            EventHandler<HostNavigateRoomEventArgs> handler = HostNavigateRoom;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostSayEventArgs> HostSay;
        protected void RaiseOnHostSay(InterceptedEventArgs e)
        {
            if (HostSay != null)
            {
                var args = new HostSayEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostSay(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostSay(HostSayEventArgs e)
        {
            EventHandler<HostSayEventArgs> handler = HostSay;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostShoutEventArgs> HostShout;
        protected void RaiseOnHostShout(InterceptedEventArgs e)
        {
            if (HostShout != null)
            {
                var args = new HostShoutEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostShout(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostShout(HostShoutEventArgs e)
        {
            EventHandler<HostShoutEventArgs> handler = HostShout;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostTradeEventArgs> HostTradePlayer;
        protected void RaiseOnHostTradePlayer(InterceptedEventArgs e)
        {
            if (HostTradePlayer != null)
            {
                var args = new HostTradeEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostTradePlayer(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostTradePlayer(HostTradeEventArgs e)
        {
            EventHandler<HostTradeEventArgs> handler = HostTradePlayer;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<HostWalkEventArgs> HostWalk;
        protected void RaiseOnHostWalk(InterceptedEventArgs e)
        {
            if (HostWalk != null)
            {
                var args = new HostWalkEventArgs(e.Continuation, e.Step, e.Packet);
                OnHostWalk(args);

                e.Cancel = args.Cancel;
                e.WasContinued = args.WasContinued;
            }
        }
        protected virtual void OnHostWalk(HostWalkEventArgs e)
        {
            EventHandler<HostWalkEventArgs> handler = HostWalk;
            if (handler != null) handler(this, e);
        }
        #endregion

        private readonly Stack<HMessage> _outPrevious, _inPrevious;
        private readonly IDictionary<ushort, Action<InterceptedEventArgs>> _outLocked, _inLocked;
        private readonly IDictionary<ushort, EventHandler<InterceptedEventArgs>> _inAttaches, _outAttaches;

        /// <summary>
        /// Gets or sets a value that determines whether to begin identifying outgoing events.
        /// </summary>
        public bool DetectOutgoing { get; set; }
        /// <summary>
        /// Gets or sets a value that determines whether to begin identifying incoming events.
        /// </summary>
        public bool DetectIncoming { get; set; }
        public bool IsDisposed { get; private set; }

        public Incoming IncomingHeaders { get; }
        public Outgoing OutgoingHeaders { get; }

        public HTriggers(bool isDetecting)
        {
            DetectOutgoing = isDetecting;
            DetectIncoming = isDetecting;

            IncomingHeaders = new Incoming();
            OutgoingHeaders = new Outgoing();

            _inPrevious = new Stack<HMessage>();
            _outPrevious = new Stack<HMessage>();

            _inLocked = new Dictionary<ushort, Action<InterceptedEventArgs>>();
            _outLocked = new Dictionary<ushort, Action<InterceptedEventArgs>>();
            _inAttaches = new Dictionary<ushort, EventHandler<InterceptedEventArgs>>();
            _outAttaches = new Dictionary<ushort, EventHandler<InterceptedEventArgs>>();
        }

        public void OutDetach()
        {
            _outAttaches.Clear();
        }
        public void OutDetach(ushort header)
        {
            if (_outAttaches.ContainsKey(header))
                _outAttaches.Remove(header);
        }
        public void OutAttach(ushort header, EventHandler<InterceptedEventArgs> callback)
        {
            _outAttaches[header] = callback;
        }

        public void InDetach()
        {
            _inAttaches.Clear();
        }
        public void InDetach(ushort header)
        {
            if (_inAttaches.ContainsKey(header))
                _inAttaches.Remove(header);
        }
        public void InAttach(ushort header, EventHandler<InterceptedEventArgs> callback)
        {
            _inAttaches[header] = callback;
        }

        public void HandleOutgoing(InterceptedEventArgs e)
        {
            if (e.Packet?.IsCorrupted ?? true) return;

            e.Packet.Position = 0;
            bool ignoreCurrent = false;
            try
            {
                if (_outAttaches.ContainsKey(e.Packet.Header))
                    _outAttaches[e.Packet.Header](this, e);

                if (DetectOutgoing && _outPrevious.Count > 0)
                {
                    e.Packet.Position = 0;
                    HMessage previous = _outPrevious.Pop();

                    if (!_outLocked.ContainsKey(e.Packet.Header) &&
                        !_outLocked.ContainsKey(previous.Header))
                        ignoreCurrent = HandleOutgoing(e.Packet, previous);
                    else ignoreCurrent = true;

                    if (ignoreCurrent)
                    {
                        e.Packet.Position = 0;
                        previous.Position = 0;

                        if (_outLocked.ContainsKey(e.Packet.Header))
                            _outLocked[e.Packet.Header](e);
                        else if (_outLocked.ContainsKey(previous.Header))
                        {
                            var args = new InterceptedEventArgs(previous);
                            _outLocked[previous.Header](args);
                        }
                    }
                }
            }
            finally
            {
                e.Packet.Position = 0;

                if (!ignoreCurrent && DetectOutgoing)
                    _outPrevious.Push(e.Packet);
            }
        }
        protected virtual bool HandleOutgoing(HMessage current, HMessage previous)
        {
            if (current.Length == 6)
            {
                // Range: 6
                if (TryHandleAvatarMenuClick(current, previous)) return true;
                if (TryHandleHostExitRoom(current, previous)) return true;
            }
            else if (current.Length >= 36 && current.Length <= 50)
            {
                //Range: 36 - 50
                if (TryHandleHostRaiseSign(current, previous)) return true;
                if (TryHandleHostNavigateRoom(current, previous)) return true;
            }
            return false;
        }

        public void HandleIncoming(InterceptedEventArgs e)
        {
            if (e.Packet?.IsCorrupted ?? true) return;

            e.Packet.Position = 0;
            bool ignoreCurrent = false;
            try
            {
                if (_inAttaches.ContainsKey(e.Packet.Header))
                    _inAttaches[e.Packet.Header](this, e);

                if (DetectIncoming && _inPrevious.Count > 0)
                {
                    e.Packet.Position = 0;
                    HMessage previous = _inPrevious.Pop();

                    if (!_inLocked.ContainsKey(e.Packet.Header) &&
                        !_inLocked.ContainsKey(previous.Header))
                        ignoreCurrent = HandleIncoming(e.Packet, previous);
                    else ignoreCurrent = true;

                    if (ignoreCurrent)
                    {
                        e.Packet.Position = 0;
                        previous.Position = 0;

                        if (_inLocked.ContainsKey(e.Packet.Header))
                            _inLocked[e.Packet.Header](e);
                        else if (_inLocked.ContainsKey(previous.Header))
                        {
                            var args = new InterceptedEventArgs(previous);
                            _inLocked[previous.Header](args);
                        }
                    }
                }
            }
            finally
            {
                e.Packet.Position = 0;

                if (!ignoreCurrent && DetectIncoming)
                    _inPrevious.Push(e.Packet);

            }
        }
        protected virtual bool HandleIncoming(HMessage current, HMessage previous)
        {
            if (current.Length == 6)
            {
                // Range: 6
                if (TryHandlePlayerKickHost(current, previous)) return true;
            }
            return false;
        }

        private bool TryHandleHostExitRoom(HMessage current, HMessage previous)
        {
            if (previous.Length != 2 || current.ReadInteger(0) != -1)
                return false;

            OutgoingHeaders.HostExitRoom = previous.Header;
            _outLocked[previous.Header] = RaiseOnHostExitRoom;
            return true;
        }
        private bool TryHandleHostRaiseSign(HMessage current, HMessage previous)
        {
            bool isHostRaiseSign = false;
            if (current.CanRead<string>(22) && current.ReadString(22) == "sign")
            {
                OutgoingHeaders.RaiseSign = previous.Header;
                _outLocked[previous.Header] = RaiseOnHostRaiseSign;

                isHostRaiseSign = true;
            }
            return isHostRaiseSign;
        }
        private bool TryHandlePlayerKickHost(HMessage current, HMessage previous)
        {
            bool isPlayerKickHost = (current.ReadInteger(0) == 4008);
            if (isPlayerKickHost)
            {
                IncomingHeaders.PlayerKickHost = current.Header;
                _inLocked[current.Header] = RaiseOnPlayerKickHost;
            }
            return isPlayerKickHost;
        }
        private bool TryHandleAvatarMenuClick(HMessage current, HMessage previous)
        {
            if (!previous.CanRead<string>(22)) return false;
            switch (previous.ReadString(22))
            {
                case "sit":
                case "stand":
                {
                    OutgoingHeaders.UpdateStance = current.Header;
                    _outLocked[current.Header] = RaiseOnHostUpdateStance;
                    return true;
                }

                case "dance_stop":
                case "dance_start":
                {
                    OutgoingHeaders.Dance = current.Header;
                    _outLocked[current.Header] = RaiseOnHostDance;
                    return true;
                }

                case "wave":
                case "idle":
                case "laugh":
                case "blow_kiss":
                {
                    OutgoingHeaders.Gesture = current.Header;
                    _outLocked[current.Header] = RaiseOnHostGesture;
                    return true;
                }
            }
            return false;
        }
        private bool TryHandleHostNavigateRoom(HMessage current, HMessage previous)
        {
            if (previous.Length >= 12 &&
                current.CanRead<string>(0) &&
                current.ReadString() == "Navigation")
            {
                current.ReadString();
                if (current.ReadString() != "go.official") return false;

                if (previous.ReadInteger(0).ToString() == current.ReadString())
                {
                    OutgoingHeaders.NavigateRoom = previous.Header;

                    _outLocked[previous.Header] = RaiseOnHostNavigateRoom;
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
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _inPrevious.Clear();
                    _outPrevious.Clear();

                    _inLocked.Clear();
                    _outLocked.Clear();

                    _inAttaches.Clear();
                    _outAttaches.Clear();

                    SKore.Unsubscribe(ref HostBanPlayer);
                    SKore.Unsubscribe(ref HostUpdateClothes);
                    SKore.Unsubscribe(ref HostUpdateMotto);
                    SKore.Unsubscribe(ref HostUpdateStance);
                    SKore.Unsubscribe(ref HostClickPlayer);
                    SKore.Unsubscribe(ref HostDance);
                    SKore.Unsubscribe(ref HostGesture);
                    SKore.Unsubscribe(ref HostKickPlayer);
                    SKore.Unsubscribe(ref HostMoveFurniture);
                    SKore.Unsubscribe(ref HostMutePlayer);
                    SKore.Unsubscribe(ref HostRaiseSign);
                    SKore.Unsubscribe(ref HostExitRoom);
                    SKore.Unsubscribe(ref HostNavigateRoom);
                    SKore.Unsubscribe(ref HostSay);
                    SKore.Unsubscribe(ref HostShout);
                    SKore.Unsubscribe(ref HostTradePlayer);
                    SKore.Unsubscribe(ref HostWalk);

                    SKore.Unsubscribe(ref FurnitureLoad);
                    SKore.Unsubscribe(ref EntityAction);
                    SKore.Unsubscribe(ref PlayerUpdate);
                    SKore.Unsubscribe(ref PlayerDance);
                    SKore.Unsubscribe(ref EntityLoad);
                    SKore.Unsubscribe(ref FurnitureDrop);
                    SKore.Unsubscribe(ref PlayerGesture);
                    SKore.Unsubscribe(ref PlayerKickHost);
                    SKore.Unsubscribe(ref FurnitureMove);
                }
                IsDisposed = true;
            }
        }
    }
}