using System.Linq;
using Exiled.Events.EventArgs;
using Player = Exiled.API.Features.Player;
using MEC;
using Hints;
using UnityEngine;
using Exiled.API.Features;
using System;
using CommandSystem;
using PlayerEvents = Exiled.Events.Handlers.Player;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using System.Collections.Generic;

namespace DocRework
{
    public class EventHandlers
    {
        public void OnRoundStart()
        {
            Timing.CallDelayed(1.5f, () =>
            {
                foreach (Player pl in Player.List)
                    if (pl.Role == RoleType.Scp049 && !pl.GameObject.GetComponent<DocRebornComponent>())
                        pl.GameObject.AddComponent<DocRebornComponent>();
            });
        }

        public void OnRoundEnd(EndingRoundEventArgs ev)
        {
            foreach (var ply in Player.List)
            {
                if (ply.GameObject.TryGetComponent(out SCP610 scp610))
                    scp610.Destroy();
                if (ply.GameObject.TryGetComponent(out DocRebornComponent docReborn))
                    docReborn.Destroy();
            }
        }
        
        public void ChangingRole(ChangingRoleEventArgs ev)
        {
            if (!ev.Player.GameObject.GetComponent<DocRebornComponent>())
                if (ev.NewRole == RoleType.Scp049)
                    ev.Player.GameObject.AddComponent<DocRebornComponent>();
        }
    }
}
