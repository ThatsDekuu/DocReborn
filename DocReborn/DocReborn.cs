using Exiled.API.Features;
using Scp049Events = Exiled.Events.Handlers.Scp049;
using ServerEvents = Exiled.Events.Handlers.Server;
using PlayerEvents = Exiled.Events.Handlers.Player;
using MEC;
using System;

namespace DocRework
{
    public class DocRework : Plugin<Config>
    {
        public override string Author { get; } = "Tomorii";
        public override string Name { get; } = "DocReborn";
        public override string Prefix { get; } = "DocReborn";
        public override Version Version { get; } = new Version(1, 1, 3);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 34);

        public static DocRework singleton;
        public EventHandler EventHandler { get; private set; }

        public override void OnEnabled()
        {
            singleton = this;
            EventHandler = new EventHandler();

            if(!Config.IsEnabled)
            {
                Log.Info("DocRework is currently disabled.");
                return;
            }

            Log.Info("DocRework is currently enabled.");

            if(Config.HealType != 0 && Config.HealType != 1)
            {
                Config.HealType = 0;
                Log.Info("HealType is defaulted to 0 (Flat HP mode) due to incorrect HealType configuration.");
            }

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            try
            {
                Timing.KillCoroutines(EventHandler.EngageBuffHandle);
                Timing.KillCoroutines(EventHandler.CooldownOnHandle);
            }
            catch (Exception e)
            {
                Log.Error($"Unable to Kill Coroutines: {e}");
            }

            foreach (var ply in Player.Get(Team.SCP))
                if (ply.GameObject.TryGetComponent(out SCP610 scp610))
                    scp610.Destroy();

            base.OnDisabled();
        }

        private void RegisterEvents()
        {
            EventHandler = new EventHandler();

            Scp049Events.FinishingRecall += EventHandler.OnFinishingRecall;
            ServerEvents.RoundStarted += EventHandler.OnRoundStart;
            ServerEvents.EndingRound += EventHandler.OnRoundEnd;
            PlayerEvents.Hurting += EventHandler.OnPlayerHit;
        }

        private void UnregisterEvents()
        {
            Scp049Events.FinishingRecall -= EventHandler.OnFinishingRecall;
            ServerEvents.RoundStarted -= EventHandler.OnRoundStart;
            ServerEvents.EndingRound -= EventHandler.OnRoundEnd;
            PlayerEvents.Hurting -= EventHandler.OnPlayerHit;

            EventHandler = null;
        }
    }
}
