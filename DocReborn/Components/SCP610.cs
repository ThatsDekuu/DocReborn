using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Interactables.Interobjects.DoorUtils;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DocRework
{
    public class SCP610 : MonoBehaviour
    {
        public Player scpPlayer { get; private set; }
        private CoroutineHandle InfectionAuraHandle = new CoroutineHandle();
        PlayerEffect speedEffect;
        private int MaxHealth = DocReborn.singleton.Config.MaxHealth;
        private float Health = DocReborn.singleton.Config.Health;
        private string SpawnHint = DocReborn.singleton.Config.SpawnBroadcast;
        private ushort HintDur = DocReborn.singleton.Config.BroadcastDuration;
        private float DamageAmount = DocReborn.singleton.Config.HitDamage;
        private byte SpeedAmount = DocReborn.singleton.Config.SpeedAmount;
        private bool InfectionMode = DocReborn.singleton.Config.InfectionMode;
        private bool AssaultMode = DocReborn.singleton.Config.AssaultMode;
        private bool InfectionAura = DocReborn.singleton.Config.InfectionAura;
        private float Range = DocReborn.singleton.Config.InfectionAuraRange;
        private float Probability = DocReborn.singleton.Config.InfectionProbability;
        private string InfectionAuraHint = DocReborn.singleton.Config.InfectionAuraBroadcast;
        private ushort InfectionAuraDur = DocReborn.singleton.Config.InfectionAuraBroadcastDuration;
        private float InfectionAuraCooldown = DocReborn.singleton.Config.InfectionAuraCooldown;

        private void Awake() 
        {
            RegisterEvents();
            scpPlayer = Player.Get(gameObject);

            speedEffect = scpPlayer.GetEffect(EffectType.Scp207);
            speedEffect.Intensity = SpeedAmount;
        }

        private void Start()
        {
            scpPlayer.Role = RoleType.Scp0492;
            scpPlayer.MaxHealth = MaxHealth;
            scpPlayer.Health = Health;
            scpPlayer.Broadcast(HintDur, SpawnHint);
            scpPlayer.CustomInfo = $"<color=#ff0000>SCP-610</color>";

            if (InfectionAura) InfectionAuraHandle = Timing.RunCoroutine(InfectionAuraEffect());
        }

        private void Update()
        {
            if (scpPlayer == null || scpPlayer.Role != RoleType.Scp0492)
                Destroy();

            if (!speedEffect.Enabled)
                scpPlayer.ReferenceHub.playerEffectsController.EnableEffect(speedEffect);
        }

        private void OnHurt(HurtingEventArgs ev)
        {
            if (ev.Attacker == scpPlayer)
            {
                if (AssaultMode && !InfectionMode) ev.Amount = DamageAmount;
                else if (InfectionMode && !AssaultMode)
                {
                    ev.Target.Role = RoleType.Scp0492;
                    ev.Target.Position = ev.Attacker.Position;
                }
                else ev.IsAllowed = false;
            }
            if (ev.Target == scpPlayer)
                if (ev.DamageType == DamageTypes.Scp207)
                    ev.Amount = 0;
        }

        private void OnDestroy() => PartiallyDestroy();

        private void PartiallyDestroy()
        {
            UnregisterEvents();
            Timing.KillCoroutines(InfectionAuraHandle);
            scpPlayer.CustomInfo = "";

            if (scpPlayer == null) return;

            speedEffect.ServerDisable();
        }

        public void Destroy()
        {
            try
            {
                Destroy(this);
            }
            catch (Exception e)
            {
                Log.Error($"Cannot destroy: {e}");
            }
        }

        private void RegisterEvents() => Exiled.Events.Handlers.Player.Hurting += OnHurt;

        private void UnregisterEvents() => Exiled.Events.Handlers.Player.Hurting -= OnHurt;

        private IEnumerator<float> InfectionAuraEffect()
        {
            while (Round.IsStarted)
            {
                if (scpPlayer != null)
                    foreach (Player p in Player.List)
                        if (Vector3.Distance(scpPlayer.Position, p.Position) <= Range && p.Team != Team.SCP && !p.IsGodModeEnabled)
                            if (UnityEngine.Random.Range(0, 101) <= Probability)
                            {
                                p.Role = RoleType.Scp0492;
                                p.Position = scpPlayer.Position;
                                p.Broadcast(InfectionAuraDur, InfectionAuraHint);
                            }
                yield return Timing.WaitForSeconds(InfectionAuraCooldown);
            }
        }
    }
}
