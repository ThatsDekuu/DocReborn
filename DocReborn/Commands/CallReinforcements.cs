using CommandSystem;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocRework.Commands
{
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
