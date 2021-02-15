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
        internal static Vector3 spawnPos = new Vector3((float)0.26, (float)1001.33, (float)-4.52);
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
}
