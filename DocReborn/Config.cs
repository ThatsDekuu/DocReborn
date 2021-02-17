using System.ComponentModel;
using Exiled.API.Interfaces;
using UnityEngine;

namespace DocRework
{
    public class Config : IConfig
    {

        /*            
         * PLUGIN
        */

        [Description("Enable or disable DocRework's mechanics")]
        public bool IsEnabled { get; set; } = true;

        /*
         *  DOCTOR PASSIVE
        */

        [Description("Allow SCP-049 to be healed for a percentage of it's missing health every player revival")]
        public bool AllowDocSelfHeal { get; set; } = true;

        [Description("Set the minimum cure amount for the buff area to kick in")]
        public int MinCures { get; set; } = 1;

        [Description("Change between 049's arua's heal type: 0 is for flat HP, 1 is for missing % HP")]
        public byte HealType { get; set; } = 0;

        [Description("Size of 049's healing radius")]
        public float HealRadius { get; set; } = 2.6f;

        [Description("The amount of HP the Doc heals their Zombies")]
        public float HealAmountFlat { get; set; } = 15.0f;

        [Description("The base amount of missing % HP the Doc heals their Zombies at the start of their buff")]
        public float ZomHealAmountPercentage { get; set; } = 10.0f;

        [Description("Multiplier for the ZomHealAmountPercentage value every time a Doctor revives someone")]
        public float HealPercentageMultiplier { get; set; } = 1.3f;

        [Description("Percentage of SCP-049's missing health to be healed")]
        public float DocMissingHealthPercentage { get; set; } = 15.0f;

        /*
         * DOCTOR ACTIVE
         */

        [Description("Cooldown for SCP049 active ability")]
        public ushort Cooldown { get; set; } = 180;

        /*
         * ZOMBIE PASSIVE
         */

        [Description("Allow SCP-049-2 to damage everyone around upon hitting an enemy target")]
        public bool AllowZombieAoe { get; set; } = true;

        [Description("Amount of health each person in 049-2's range loses by 049-2's AOE attack")]
        public float ZombieAoeDamage { get; set; } = 15.0f;

        /*
         * TRANSLATIONS 
         */

        [Description("Message sent to SCP-049 upon reaching the minimum cures amount required")]
        public string Translation_Passive_ActivationMessage { get; set; } = "<color=#ff0000>Your passive ability is now activated.\nYou now heal zombies around you every 5 seconds.</color>";

        [Description("Message sent when you try to execute the .cr command when you're not a doctor")]
        public string Translation_Active_PermissionDenied { get; set; } = "You are not allowed to use this command!";

        [Description("Message sent when you try to execute the .cr command but you don't yet have the min required revives")]
        public string Translation_Active_NotEnoughRevives { get; set; } = "You don't have enough revives to use this ability!";

        [Description("Message sent when you try to execute the .cr command while it's on cooldown")]
        public string Translation_Active_OnCooldown { get; set; } = "Can't use this yet! Cooldown remaining: ";

        [Description("Message send when there are no spectators to spawn")]
        public string Translation_Active_NoSpectators { get; set; } = "There're no Spectators, wait until someone dies.";

        [Description("Hint displayed when the .cr ability's cooldown has expired")]
        public string Translation_Active_ReadyNotification { get; set; } = "<color=#00ff00>You can now use your active ability.\nUse .cr in your console to spawn a zombie from the spectators.</color>";
        [Description("Console response displayed when you're calling the .cr ability")]
        public string Translation_Success_CallReinforcement { get; set; } = "<color=#ff0000>Reinforcements are on the way</color>";

        /*
         * SCP-610
         */

        [Description("Enables the possibility to become SCP-610")]
        public bool Scp610Enabled { get; set; } = true;

        [Description("The probability to spawn a player as SCP-610 after SCP-049 revived him")]
        public float SpawnProbability { get; set; } = 100f;

        [Description("SCP-610 Max Health")]
        public int MaxHealth { get; set; } = 700;

        [Description("The amount of health when SCP-610 spawns")]
        public float Health { get; set; } = 700;

        [Description("The message will be displayed when SCP-610 spawns")]
        public string SpawnBroadcast { get; set; } = "<size=60><b><color=#ffffff>You are</color> <color=#ff0000>SCP-610</color></b></size>\n<size=50><b><color=#ffffff>You are more</color> <color=#ff0000>resistant</color> <color=#ffffff>and more</color> <color=#ff0000>dangerous</color></b></size>";

        [Description("The message duration will be displayed when SCP-610 spawns")]
        public ushort BroadcastDuration { get; set; } = 10;

        [Description("The SCP-610 Speed (Value Between 1-4 | Based on SCP-207 Intensity)")]
        public byte SpeedAmount { get; set; } = 2;

        [Description("The amount of damage when SCP-610 hits a player")]
        public float HitDamage { get; set; } = 50f;

        [Description("Enable the Infection Mode, each player touched by SCP-610 will become SCP-049-2")]
        public bool InfectionMode { get; set; } = false;

        [Description("Enable the Assault Mode, SCP-610 can't infect anyone, but he will deal a lot of damage")]
        public bool AssaultMode { get; set; } = true;

        [Description("Enable the Infection Aura, each player inside the range will become SCP-049-2")]
        public bool InfectionAura { get; set; } = false;

        [Description("The Infection Aura radius where you can be affected by")]
        public float InfectionAuraRange { get; set; } = 2f;

        [Description("Percentage to become SCP-049-2 when a player is inside the Infection Aura radius")]
        public float InfectionProbability { get; set; } = 30f;

        [Description("The Infection Aura cooldown (E.g. When you're inside the radius and the Infection Aura is in cooldown mode you won't be infected)")]
        public float InfectionAuraCooldown { get; set; } = 5f;

        [Description("The message will be displayed to the player infected by the Infection Aura")]
        public string InfectionAuraBroadcast { get; set; } = "<size=60><b><color=#ffffff>You have been</color> <color=#ff0000>infected</color> <color=#ffffff>with</color> <color=#ff0000>SCP-610</color></b></size>";

        [Description("The message duration will be displayed to the player infected by the Infection Aura")]
        public ushort InfectionAuraBroadcastDuration { get; set; } = 5;

        [Description("The zone where SCP-610 will be spawned with the command")]
        public float Xpos { get; set; } = 0.26f;
        public float Ypos { get; set; } = 1001.33f;
        public float Zpos { get; set; } = -4.52f;
    }
}
