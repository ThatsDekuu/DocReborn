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
    public class SCP049AbilityController
    {
        public static uint CureCounter = 0;
        public static ushort AbilityCooldown;
        private static float Radius = DocRework.singleton.Config.HealRadius;
        private static float HealAmountFlat = DocRework.singleton.Config.HealAmountFlat;
        public static float HealAmountPercentage = DocRework.singleton.Config.ZomHealAmountPercentage;
        private static byte HealType = DocRework.singleton.Config.HealType;
        public static IEnumerator<float> EngageBuff(Player ply)
        {
            while (Round.IsStarted)
            {
                foreach (Player Zm in Player.Get(RoleType.Scp0492))
                    if (Vector3.Distance(ply.Position, Zm.Position) <= Radius)
                        ApplyHeal(HealType, Zm, HealAmountFlat, HealAmountPercentage);

                yield return Timing.WaitForSeconds(5f);
            }
        }

        private static void ApplyHeal(byte type, Player p, float flat, float multiplier)
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

        public static void CallZombieReinforcement(Player ply, ushort cd, List<Player> pList)
        {  
            var index = 0;
            index += new System.Random().Next(pList.Count);
            var chosenPlayer = pList[index];

            chosenPlayer.Role = RoleType.Scp0492;
            chosenPlayer.Health = chosenPlayer.MaxHealth;

            Timing.CallDelayed(0.5f, () => chosenPlayer.Position = ply.Position);

            index = 0;

            AbilityCooldown = DocRework.singleton.Config.Cooldown;
            EventHandler.CooldownOnHandle = Timing.RunCoroutine(StartCooldownTimer());
        }

        public static IEnumerator<float> StartCooldownTimer()
        {
            while(AbilityCooldown != 0)
            {
                AbilityCooldown--;
                yield return Timing.WaitForSeconds(1f);
            }

            IEnumerable<Player> scp049List = Player.Get(RoleType.Scp049);
            foreach (Player s049 in scp049List)
                s049.ShowHint(DocRework.singleton.Config.Translation_Active_ReadyNotification, 5f);

            Timing.KillCoroutines(EventHandler.CooldownOnHandle);
        }
    }
}
