# Description:
This mod adds an RPG-like system of levels and attribute increases: - Wacky Branch 1.4.0
WackyEpicMMOSystem release until author comes back. 

Wacky Git https://github.com/Wacky-Mole/WackyEpicMMOSystem

Original git - https://github.com/Single-sh/EpicMMOSystem
![https://i.imgur.com/5Tzgs0R.png](https://i.imgur.com/5Tzgs0R.png)

Features:
 - Shared group XP. Outside of groups all XP awards go to the character who struck the last blow.
 - Custom mobs can be added for XP gain.
 - MMO-like friends list. -Groups MOD
 - On screen XP bar.
 - Compatible with [ItemRequiresSkillLevel](https://valheim.thunderstore.io/package/Detalhes/ItemRequiresSkillLevel/) mod. Equipment can be limited by level or attribute.
 - Compatible with KGMarketplace mod. Experience rewards can be added: (EpicMMO_Exp:250) Quests can be limited by level (EpicMMO_Level: text, 20)
 
 ![https://i.imgur.com/lkCcVOo.png](https://i.imgur.com/lkCcVOo.png)

<details><summary>Attributes</summary>

	Strength: Physical Damage increase, Carry Weight Increase, 

	Agility: Attack Stamina Consumption decrease, Stamina increase, Stamina consumption (running, jumping) decreased

	Intellect: Elemental Damage increase, Elemental Armor increase, (In future updates Eitrl regen)

	Endurance: Physical Armor increase, HP increase, Health Regeneration

<details><summary>Friends list</summary>

If you want people to be a part of your Group to earn XP, download the Group mod for each client https://valheim.thunderstore.io/package/Smoothbrain/Groups/

Click the plus button at the bottom of the friends bar. Enter the name of the character you wish to add, starting with a capital letter.
   ![https://i.imgur.com/rC8RDYe.png](https://i.imgur.com/rC8RDYe.png)
The player will receive a friend request. Once accepted, the character will appear in your friends list. Group invites can be sent from the friends list. 
   ![https://i.imgur.com/W460hdu.png](https://i.imgur.com/W460hdu.png)

# Warning: 
- If you accept a friend request while the player who sent it is not logged in with the character, you will not be added to their friends list and they will need to resend the friend request.
- You cannot send friend requests to yourself or characters you have already added. If you need to send another friend request, remove the character from the list first.
- Friend requests that have been sent, but not accepted will be removed on logout. They must be accepted while both characters are online.
</details> 

<details><summary>Creature level control</summary>

This mod assigns levels to all in-game monsters.
![https://i.imgur.com/IySsj3j.png](https://i.imgur.com/IySsj3j.png)
Monsters that are 1 level higher than the character (+ value from config) do not reward XP. Damage dealt to a higher level monster will be reduced by the difference in levels. E.g. (Character level 20/ Monster level 50 = 0.4. Damage dealt will be 0.4% of normal damage) Higher level monsters will have their names appear in red.

If you are significantly higher level than a monster, your XP award will be reduced. Monsters that are significantly lower level than you will have their names appear in cyan.

All of these formulas functions can be configured in the settings file.
A file listing all monsters and their levels is located in plugin/EpicMMOSystem/MonsterDB_"Version".

Please note:
When upgrading the mod to a newer version, new fields in the settings file will be created automatically. You will have to manually re-edit these values if you have changed them.
If you have no custom settings in the configuration file, you should delete the file so that a fresh one can be created by the new version.
Note for other Mods: This mod uses hit.toolTier to pass the Lvl of player



</details>


<details><summary>Console commands</summary>

Admin only commands: - Should work in singleplayer now
 - To set a character's level: `epicmmosystem level [value] [name]` - (If the name contains a space, replace the space with the ampersand (&) symbol)
 - To reset attribute points: `epicmmosystem reset_points [name]` - (If the name contains a space, replace the space with the ampersand (&) symbol)
</details> 

<details><summary>Feedback</summary>

For questions or suggestions please join my discord channel: [Odin Plus Team](https://discord.gg/odinplus) -WackyMole on Odins

Original Creator: LambaSun or my [mod branch](https://discord.com/channels/826573164371902465/977656428670111794)

</details> 

<details><summary>Changelog</summary>
 
 - 1.4.0. Fix for inventory to bag JC (hopefully), Changed Configs,PLEASE DELETE OLD CONFIGS!, added removeDropMax, removeDropMax,removeBossDropMax, removeBossDropMix, curveExp, curveBossExp. 
		  Allow for multiple Jsons to be searched. Added admin rights to singleplayer hosting. Boss drop is determined by mob.faction(), curveBossExp Exp is just the 6 main bosses.  
		  Updated Monster.json moved to configs instead of plugin. Added ExtraDebugmode for future issues. Updated MonserDB_Default for mistlands,
		  LandAnimals mod, MonsterLabZ, Outsiders, SeaAnimals, Fantasy Creatures, Air Animals, and Outsiders. Json file in MMO folder is searched.
		  Added Version text to easily update in future. Write "NO" in Ver.txt to skip future updates. Moved Monster lvl bar [] for boss and non boss
 - 1.3.1: Dual wield and EpicMMO Thanks to KG, sponsored by Aldhari/Skaldhari
 - 1.3.0: WackyEpicMMOSystem release, until author comes back. Code from Azumatt - Updated Chat, Group and ServerSync
 - 1.2.8: Added a limiter for the maximum attribute value. New view health and stamina bar (in the configuration you can return the old display where only the experience is displayed).
 - 1.2.7: Fix version check
 - 1.2.6: Fixed bug of different amount of experience. Added ability to add your own items or currency to reset attributes.
 - 1.2.5: Fix damage monsters and fix error for friends list
 - 1.2.4: Fix version check
 - 1.2.3: Add console command and xp loss on death
 - 1.2.2: Add button to open the quest journal (Marketplace) and profession window
 - 1.2.1: Fix errors with EAQS
 - 1.2.0: Add friends list feature
 - 1.1.0: Add creature level control
 - 1.0.1: Fix localization and append english text for config comments.
 - 1.0.0: Release
</details> 