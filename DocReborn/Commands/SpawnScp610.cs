using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using Interactables.Interobjects.DoorUtils;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DocRework.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class SpawnScp610 : ICommand
    {
        string ICommand.Command { get; } = "scp610";
        string[] ICommand.Aliases { get; } = new string[] { "s610, spawn610, sscp610" };
        string ICommand.Description { get; } = "Spawn a player as SCP-610!";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!((CommandSender)sender).CheckPermission("doc.spawn610"))
            {
                response = "You do not have permission to execute this command.";

                return false;
            }

            if (arguments.Count != 1)
            {
                response = "Usage: scp610 [Player Id]";

                return false;
            }

            if (int.TryParse(arguments.At(0), out int id))
            {
                Player plr = Player.Get(arguments.At(0));

                if (plr == null)
                {
                    response = $"Player not found";
                    return false;
                }

                plr.GameObject.AddComponent<SCP610>();
                Timing.CallDelayed(0.3f, () => plr.Position = EventHandler.spawnPos);
            }

            response = "Player spawned.";

            return true;
        }
    }
}
