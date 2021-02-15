using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DocRework
{
    public class SCP0492AbilityController
    {
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
