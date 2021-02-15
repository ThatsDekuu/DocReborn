using System.Linq;
using Exiled.Events.EventArgs;
using Player = Exiled.API.Features.Player;
using MEC;
using Hints;
using UnityEngine;
using Exiled.API.Features;
using System;
using CommandSystem;

namespace DocRework
{
    public class EventHandler
    {
        private bool AllowDocHeal = DocRework.singleton.Config.AllowDocSelfHeal;
        private float MissingHealthPercentage = DocRework.singleton.Config.DocMissingHealthPercentage;
        private bool EnableZombieAOEDamage = DocRework.singleton.Config.AllowZombieAoe;
        private float ZombieAOEDamage = DocRework.singleton.Config.ZombieAoeDamage;
        internal static CoroutineHandle EngageBuffHandle = new CoroutineHandle();
        internal static CoroutineHandle CooldownOnHandle = new CoroutineHandle();
        public Player Scp049Ply { get; private set; }

        public void OnFinishingRecall(FinishingRecallEventArgs ev)
        {
            if (!RoundSummary.RoundInProgress()) return;

            SCP049AbilityController.CureCounter++;

            if (DocRework.singleton.Config.Scp610Enabled && UnityEngine.Random.Range(0, 101) <= DocRework.singleton.Config.SpawnProbability)
            {
                ev.Target.GameObject.AddComponent<SCP610>();
                ev.Target.Position = ev.Scp049.Position;
            }

            if (SCP049AbilityController.CureCounter == DocRework.singleton.Config.MinCures)
            {
                Scp049Ply.ShowHint(DocRework.singleton.Config.Translation_Passive_ActivationMessage, 5f);

                EngageBuffHandle = Timing.RunCoroutine(SCP049AbilityController.EngageBuff(Scp049Ply));
                CooldownOnHandle = Timing.RunCoroutine(SCP049AbilityController.StartCooldownTimer());
            }

            if (DocRework.singleton.Config.HealType == 1 && SCP049AbilityController.CureCounter > DocRework.singleton.Config.MinCures)
                SCP049AbilityController.HealAmountPercentage *= DocRework.singleton.Config.HealPercentageMultiplier;

            if(AllowDocHeal) SCP049AbilityController.ApplySelfHeal(ev.Scp049, MissingHealthPercentage);
        }

        public void OnPlayerHit(HurtingEventArgs ev)
        {
            if (EnableZombieAOEDamage && SCP049AbilityController.CureCounter >= DocRework.singleton.Config.MinCures)
                SCP0492AbilityController.DealAOEDamage(ev.Attacker, ev.Target, ZombieAOEDamage);
        }

        public void OnRoundStart()
        {
            foreach (Player ply in Player.Get(RoleType.Scp049))
                Scp049Ply = ply;
        }

        public void OnRoundEnd(EndingRoundEventArgs ev)
        {
            try
            {
                Timing.KillCoroutines(EngageBuffHandle);
                Timing.KillCoroutines(CooldownOnHandle);
            }
            catch (Exception e)
            {
                Log.Error($"Unable to Kill Coroutines: {e}");
            }

            SCP049AbilityController.CureCounter = 0;
            SCP049AbilityController.AbilityCooldown = DocRework.singleton.Config.Cooldown;

            foreach (var ply in Player.Get(Team.SCP))
                if (ply.GameObject.TryGetComponent(out SCP610 scp610))
                    scp610.Destroy();
        }
    }

    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class CallReinforcement : ICommand
    {
        string ICommand.Command { get; } = "cr";
        string[] ICommand.Aliases { get; } = new[] { "cre" };
        string ICommand.Description { get; } = "Call a Zombie Reinforcement from Spectators!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender)sender).SenderId);
            var pList = Player.Get(Team.RIP).ToList();

            if (player.Role != RoleType.Scp049)
            {
                response = DocRework.singleton.Config.Translation_Active_PermissionDenied;
                return false;
            }
            if (SCP049AbilityController.CureCounter < DocRework.singleton.Config.MinCures)
            {
                response = DocRework.singleton.Config.Translation_Active_NotEnoughRevives;
                return false;
            }
            if (SCP049AbilityController.AbilityCooldown > 0)
            {
                response = DocRework.singleton.Config.Translation_Active_OnCooldown + SCP049AbilityController.AbilityCooldown;
                return false;
            }
            if (pList.IsEmpty())
            {
                response = DocRework.singleton.Config.Translation_Active_OnCooldown + SCP049AbilityController.AbilityCooldown;
                return false;
            }

            SCP049AbilityController.CallZombieReinforcement(player, SCP049AbilityController.AbilityCooldown, pList);
            response = DocRework.singleton.Config.Translation_Success_CallReinforcement;
            return true;
        }
    }

    
}
