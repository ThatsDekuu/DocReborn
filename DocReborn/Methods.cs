using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Hints;
using MEC;
using UnityEngine;
using Player = Exiled.API.Features.Player;

namespace DocRework
{
    public class Methods
    {
        public static void ApplyHeal(byte type, Player p, float flat, float multiplier)
        {
            float HpGiven;
            bool CanDisplay = true;
            float MissingHP = p.MaxHealth - p.Health;

            if (p.Health == p.MaxHealth)
                CanDisplay = false;

            if (type == 0)
            {
                if (p.Health + flat > p.MaxHealth)
                {
                    HpGiven = p.MaxHealth - p.Health;
                    p.Health = p.MaxHealth;
                }
                else
                {
                    HpGiven = flat;
                    p.Health += flat;
                }

            }
            else
            {
                if (p.Health + MissingHP * multiplier > p.MaxHealth)
                {
                    HpGiven = p.MaxHealth - p.Health;
                    p.Health = p.MaxHealth;
                }
                else
                {
                    HpGiven = MissingHP * multiplier;
                    p.Health += MissingHP * multiplier;
                }
            }

            if(CanDisplay) p.ShowHint($"<color=#ff0000>+{HpGiven} HP</color>", 2f);
        }

        public static void ApplySelfHeal(Player p, float missing)
        {
            float MissingHP = p.MaxHealth - p.Health;
            if (p.Health + MissingHP * missing > p.MaxHealth) p.Health = p.MaxHealth; 
            else p.Health += MissingHP * missing;
        }

        public static void CallZombieReinforcement(Vector3 pos, ushort AbilityCooldown, List<Player> pList)
        {
            var chosenPlayer = pList[new System.Random().Next(pList.Count)];

            chosenPlayer.Role = RoleType.Scp0492;
            chosenPlayer.Health = chosenPlayer.MaxHealth;

            Timing.CallDelayed(0.5f, () => chosenPlayer.Position = pos);

            AbilityCooldown = DocReborn.singleton.Config.Cooldown;            
        }

        public static IEnumerator<float> StartCooldownTimer(Player ply)
        {
            while (ply.GameObject.TryGetComponent(out DocRebornComponent docReborn) && docReborn.AbilityCooldown != 0)
            {
                docReborn.AbilityCooldown--;
                yield return Timing.WaitForSeconds(1f);
            }

            foreach (Player s049 in Player.Get(RoleType.Scp049))
                s049.ShowHint(DocReborn.singleton.Config.Translation_Active_ReadyNotification, 5f);

            Timing.KillCoroutines(ply.GameObject.GetComponent<DocRebornComponent>().CooldownOnHandle);
        }

        public static void DealAOEDamage(Player Attacker, Player Target, float AOEDamage)
        {
            if (Attacker.Role != RoleType.Scp0492 || Target.Team == Team.SCP) return;

            IEnumerable<Player> pList = Player.List.Where(r => r.Team != Team.SCP && r != Attacker && !r.IsGodModeEnabled);
            foreach (Player P in pList)
            {
                if (Vector3.Distance(Attacker.Position, P.Position) > 1.65f) return;

                if (P.Health - AOEDamage > 0) P.Health -= AOEDamage;
                else P.Kill(DamageTypes.Scp0492);
            }
        }
    }
}
