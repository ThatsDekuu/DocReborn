using CommandSystem;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DocRework.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CallReinforcement : ICommand
    {
        public string Command { get; } = "cr";
        public string[] Aliases { get; } = new string[] { };
        public string Description { get; } = "Call a Zombie Reinforcement from Spectators!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(((CommandSender)sender).SenderId);
            var pList = Player.Get(Team.RIP).ToList();

            if (arguments.Count > 0)
            {
                response = "Usage: cr";

                return false;
            }

            if (player.Role != RoleType.Scp049)
            {
                response = DocReborn.singleton.Config.Translation_Active_PermissionDenied;
                return false;
            }
            if (player.GameObject.GetComponent<DocRebornComponent>().CureCounter < DocReborn.singleton.Config.MinCures)
            {
                response = DocReborn.singleton.Config.Translation_Active_NotEnoughRevives;
                return false;
            }
            if (player.GameObject.TryGetComponent(out DocRebornComponent docReborn) && docReborn.AbilityCooldown > 0)
            {
                response = DocReborn.singleton.Config.Translation_Active_OnCooldown + docReborn.AbilityCooldown;
                return false;
            }
            if (pList.IsEmpty())
            {
                response = DocReborn.singleton.Config.Translation_Active_NoSpectators;
                return false;
            }

            Vector3 pos = player.Position;
            Methods.CallZombieReinforcement(pos, player.GameObject.GetComponent<DocRebornComponent>().AbilityCooldown, pList);
            Timing.RunCoroutine(Methods.StartCooldownTimer(player));
            response = DocReborn.singleton.Config.Translation_Success_CallReinforcement;
            return true;
        }
    }
}
