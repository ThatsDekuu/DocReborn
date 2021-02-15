# DocReborn

Remade version of [DocRework](https://github.com/rby-blackruby/DocRework)

## Features
- After SCP-049 reaches a specified minimum amount of revives, a healing aura spawns around himself that can either heal SCP-049-2's nearby for a flat amount of hp, or a percentage of their missing health.
- Using the .cr command in your player console you can summon an SCP-049-2 from the spectators.
- Upon reviving a player, SCP-049 gains a percentage of it's missing health back.
- SCP-049-2s now deal AOE damage, making them very effective in crowded combats.
- Translation options.
- New Role: SCP-610. It's a SCP-049-2 with more health and more abilities.

## Requirements
- This plugin uses [EXILED](https://github.com/galaxy119/EXILED/).

Note: **DocRework 1.0+ requires Exiled 2.1.34**

## Configs
| Config option | Value type | Default value | Description |
| --- | --- | --- | --- |
| `IsEnabled` | bool | true | Enables the DocRework plugin. Set it to false to disable it. |
| `AllowDocSelfHeal` | bool | true | Allow SCP-049 to be healed for a percentage of it's missing health every player revival. |
| `MinCures` | int | 3 | Minimum cure amount for the buff area to kick in. |
| `HealType`| byte | 0 | Change between 049's arua's heal type: 0 is for flat HP, 1 is for missing % HP. |
| `HealRadius` | float | 2.6f | 049's area's healing radius. Don't set it to 0 or below! |
| `HealAmountFlat` | float | 15.0f | The amount of flat HP the Doc heals their zombies. Don't set it to 0 or below! |
|`ZomHealAmountPercentage` | float | 10.0f | The base amount of missing % HP the Doc heals their Zombies at the start of their buff. |
| `HealPercentageMultiplier` | float | 1.3f | Multiplier for the ZomHealAmountPercentage value every time a Doctor revives someone. |
| `DocMissingHealthPercentage` | float | 15.0f | Percentage of SCP-049's missing health to be healed. |
| `Cooldown` | ushort | 180 | Cooldown for SCP-049 active ability. |
| `AllowZombieAOE` | bool | true | Allow SCP-049-2 to damage everyone around upon hitting an enemy target. |
| `ZombieAOEDamage` | float | 15.0f | Amount of health each player in 049-2's range loses by 049-2's AOE attack. |
| `Scp610Enabled` | bool | true | Enables the possibility to become SCP-610. |
| `SpawnProbability` | float | 10f | The probability to spawn a player as SCP-610 after SCP-049 revived him. |
| `MaxHealth` | int | 700 | SCP-610 Max Health. |
| `Health`| float | 700 | The amount of health when SCP-610 spawns. |
| `SpawnBroadcast` | string | "You are SCP-610. You are more resistant and more dangerous" | The message will be displayed when SCP-610 spawns. |
| `BroadcastDuration` | ushort | 10 | The message duration will be displayed when SCP-610 spawns. |
| `SpeedAmount` | byte | 2 | The SCP-610 Speed (Value Between 1-4 | Based on SCP-207 Intensity). |
| `HitDamage` | float | 50f | The amount of damage when SCP-610 hits a player. |
| `InfectionMode` | bool | false | Enables the Infection Mode, each player touched by SCP-610 will become SCP-049-2. |
| `AssaultMode` | bool | true | Enable the Assault Mode, SCP-610 can't infect anyone, but he will deal a lot of damage. |
| `InfectionAura` | bool | false | Enable the Infection Aura, each player inside the range will become SCP-049-2. |
| `InfectionAuraRange` | float | 2f | The Infection Aura radius where you can be affected by. |
| `InfectionProbability` | float | 30f | Percentage to become SCP-049-2 when a player is inside the Infection Aura radius. |
| `InfectionAuraCooldown` | float | 5f | The Infection Aura cooldown (E.g. When you're inside the radius and the Infection Aura is in cooldown mode you won't be infected). |
| `InfectionAuraBroadcast` | string | "You have been infected with SCP-610" | The message will be displayed to the player infected by the Infection Aura. |
| `InfectionAuraBroadcastDuration` | ushort | 5 | The message duration will be displayed to the player infected by the Infection Aura. |
