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
    public class DocRebornComponent : MonoBehaviour
    {
        public Player Scp049Ply { get; private set; }
        private bool AllowDocHeal = DocReborn.singleton.Config.AllowDocSelfHeal;
        private float MissingHealthPercentage = DocReborn.singleton.Config.DocMissingHealthPercentage;
        private bool EnableZombieAOEDamage = DocReborn.singleton.Config.AllowZombieAoe;
        private float ZombieAOEDamage = DocReborn.singleton.Config.ZombieAoeDamage;
        internal CoroutineHandle EngageBuffHandle = new CoroutineHandle();
        internal CoroutineHandle CooldownOnHandle = new CoroutineHandle();
        internal uint CureCounter = 0;
        public ushort AbilityCooldown;
        private float Radius = DocReborn.singleton.Config.HealRadius;
        private float HealAmountFlat = DocReborn.singleton.Config.HealAmountFlat;
        public float HealAmountPercentage = DocReborn.singleton.Config.ZomHealAmountPercentage;
        private byte HealType = DocReborn.singleton.Config.HealType;

        private void Awake()
        {
            Load();
            Scp049Ply = Player.Get(gameObject);
        }

        private void Update()
        {
            if (Scp049Ply == null || Scp049Ply.Role != RoleType.Scp049)
                Destroy();
        }

        private void OnDestroy() => PartiallyDestroy();
        private void PartiallyDestroy()
        {
            Unload();
            Timing.KillCoroutines(EngageBuffHandle);
            Timing.KillCoroutines(CooldownOnHandle);
            if (Scp049Ply == null) return;
        }
        public void Destroy()
        {
            try
            {
                Destroy(this);
            }
            catch (Exception e)
            {
                Log.Error($"Can't Destroy: {e}");
            }
        }

        public void OnFinishingRecall(FinishingRecallEventArgs ev)
        {
            CureCounter++;

            if (DocReborn.singleton.Config.Scp610Enabled && UnityEngine.Random.Range(0, 101) <= DocReborn.singleton.Config.SpawnProbability)
            {
                ev.Target.GameObject.AddComponent<SCP610>();
                ev.Target.Position = ev.Scp049.Position;
            }

            if (CureCounter == DocReborn.singleton.Config.MinCures)
            {
                Scp049Ply.ShowHint(DocReborn.singleton.Config.Translation_Passive_ActivationMessage, 5f);

                EngageBuffHandle = Timing.RunCoroutine(EngageBuff());
                CooldownOnHandle = Timing.RunCoroutine(Methods.StartCooldownTimer(Scp049Ply));
            }

            if (DocReborn.singleton.Config.HealType == 1 && CureCounter > DocReborn.singleton.Config.MinCures)
                HealAmountPercentage *= DocReborn.singleton.Config.HealPercentageMultiplier;

            if (AllowDocHeal) Methods.ApplySelfHeal(ev.Scp049, MissingHealthPercentage);
        }

        public void OnPlayerHit(HurtingEventArgs ev)
        {
            if (EnableZombieAOEDamage && CureCounter >= DocReborn.singleton.Config.MinCures)
                Methods.DealAOEDamage(ev.Attacker, ev.Target, ZombieAOEDamage);
        }

        public void Load()
        {
            Scp049Events.FinishingRecall += OnFinishingRecall;
            PlayerEvents.Hurting += OnPlayerHit;
        }

        public void Unload()
        {
            Scp049Events.FinishingRecall -= OnFinishingRecall;
            PlayerEvents.Hurting -= OnPlayerHit;
        }

        public IEnumerator<float> EngageBuff()
        {
            while (Round.IsStarted)
            {
                foreach (Player Zm in Player.Get(RoleType.Scp0492))
                    if (Vector3.Distance(Scp049Ply.Position, Zm.Position) <= Radius)
                        Methods.ApplyHeal(HealType, Zm, HealAmountFlat, HealAmountPercentage);

                yield return Timing.WaitForSeconds(5f);
            }
        }
    }
}
