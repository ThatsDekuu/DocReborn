using Exiled.API.Features;
using ServerEvents = Exiled.Events.Handlers.Server;
using MEC;
using System;

namespace DocRework
{
    public class DocReborn : Plugin<Config>
    {
        public override string Author { get; } = "Tomorii";
        public override string Name { get; } = "DocReborn";
        public override string Prefix { get; } = "DocReborn";
        public override Version Version { get; } = new Version(1, 2, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 34);

        public static DocReborn singleton;
        public EventHandlers EventHandlers { get; private set; }

        public override void OnEnabled()
        {
            singleton = this;
            RegisterEvents();


            if (!Config.IsEnabled)
            {
                Log.Info("DocReborn is currently disabled.");
                return;
            }

            Log.Info("DocReborn is currently enabled.");

            if(Config.HealType != 0 && Config.HealType != 1)
            {
                Config.HealType = 0;
                Log.Info("HealType is defaulted to 0 (Flat HP mode) due to incorrect HealType configuration.");
            }
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            foreach (var ply in Player.List)
                if (ply.GameObject.TryGetComponent(out SCP610 scp610))
                    scp610.Destroy();
            foreach (var ply in Player.List)
                if (ply.GameObject.TryGetComponent(out DocRebornComponent docReborn))
                    docReborn.Destroy();

            base.OnDisabled();
        }

        public void RegisterEvents()
        {
            EventHandlers = new EventHandlers();

            Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.ChangingRole;
            ServerEvents.RoundStarted += EventHandlers.OnRoundStart;
            ServerEvents.EndingRound += EventHandlers.OnRoundEnd;
        }

        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.ChangingRole;
            ServerEvents.RoundStarted -= EventHandlers.OnRoundStart;
            ServerEvents.EndingRound -= EventHandlers.OnRoundEnd;

            EventHandlers = null;
        }
    }
}
