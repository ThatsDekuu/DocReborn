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
        private int MaxHealth = DocRework.singleton.Config.MaxHealth;
        private float Health = DocRework.singleton.Config.Health;
        private string SpawnHint = DocRework.singleton.Config.SpawnBroadcast;
        private ushort HintDur = DocRework.singleton.Config.BroadcastDuration;
        private float DamageAmount = DocRework.singleton.Config.HitDamage;
        private byte SpeedAmount = DocRework.singleton.Config.SpeedAmount;
        private bool InfectionMode = DocRework.singleton.Config.InfectionMode;
        private bool AssaultMode = DocRework.singleton.Config.AssaultMode;
        private bool InfectionAura = DocRework.singleton.Config.InfectionAura;
        private float Range = DocRework.singleton.Config.InfectionAuraRange;
        private float Probability = DocRework.singleton.Config.InfectionProbability;
        private string InfectionAuraHint = DocRework.singleton.Config.InfectionAuraBroadcast;
        private ushort InfectionAuraDur = DocRework.singleton.Config.InfectionAuraBroadcastDuration;
        private float InfectionAuraCooldown = DocRework.singleton.Config.InfectionAuraCooldown;

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

            if (InfectionAura)
                InfectionAuraHandle = Timing.RunCoroutine(InfectionAuraEffect(scpPlayer, Range, Probability, InfectionAuraDur, InfectionAuraHint, InfectionAuraCooldown));
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

        private IEnumerator<float> InfectionAuraEffect(Player ply, float range, float probability, ushort dur, string bc, float cooldown)
        {
            while (Round.IsStarted)
            {
                if (ply != null)
                    foreach (Player p in Player.List)
                        if (Vector3.Distance(ply.Position, p.Position) <= range && p.Team != Team.SCP && !p.IsGodModeEnabled)
                            if (UnityEngine.Random.Range(0, 101) <= probability)
                            {
                                p.Role = RoleType.Scp0492;
                                p.Position = ply.Position;
                                p.Broadcast(dur, bc);
                            }
                yield return Timing.WaitForSeconds(cooldown);
            }
        }
    }
}
