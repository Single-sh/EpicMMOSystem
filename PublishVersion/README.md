# Description:
This mod adds an RPG-like system of levels and attribute increases: - Wacky Branch 1.5.3
WackyEpicMMOSystem release until author comes back. 

Wacky Git https://github.com/Wacky-Mole/WackyEpicMMOSystem

Original git - https://github.com/Single-sh/EpicMMOSystem
![https://i.imgur.com/5Tzgs0R.png](https://i.imgur.com/5Tzgs0R.png)

Features:
 - Shared group XP. Outside of groups all XP awards go to the character who struck the last blow.
 - Custom mobs can be added for XP gain.
 - MMO-like friends list. -[Groups](https://valheim.thunderstore.io/package/Smoothbrain/Groups/)
 - On screen XP bar.
 - Compatible with [ItemRequiresSkillLevel](https://valheim.thunderstore.io/package/Detalhes/ItemRequiresSkillLevel/) mod. Equipment can be limited by level or attribute.
 - Compatible with [KGMarketplace mod](https://valheim.thunderstore.io/package/KGvalheim/Marketplace_And_Server_NPCs_Revamped/). Experience rewards can be added: (EpicMMO_Exp:250) Quests can be limited by level (EpicMMO_Level: text, 20)
 
 ![https://i.imgur.com/lkCcVOo.png](https://i.imgur.com/lkCcVOo.png)

<details><summary>Attributes</summary>

	Strength: Physical Damage increase, Carry Weight Increase, 

	Agility: Attack Stamina Consumption decrease, Stamina increase, Stamina consumption (running, jumping) decreased

	Intellect: Elemental Damage increase, Elemental Armor increase, Eitrl regen increases

	Endurance: Physical Armor increase, HP increase, Health Regeneration

</details> 

<details><summary>Friends list</summary>

MMO-like friends list. -Groups MOD Group to earn XP, download requires Group mod for each client https://valheim.thunderstore.io/package/Smoothbrain/Groups/

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

Mobs from other mods are included:

Fantasy-Creatures, AirAnimals, Defaults, DoOrDieMonsters, LandAnimals, MonsterlabZ, Outsiders, SeaAnimals

Monsters that are 1 level higher than the character + MaxLevelRange will curve XP.

With Low_damage_level- Damage dealt to a higher level monster will be reduced by the difference in levels. E.g. (Character level 20/ Monster level 50 = 0.4. Damage dealt will be 0.4% of normal damage) 
damageFactor = (float)(playerLevel + LowDamageConfig)/ monsterLevel; You can configure LowDamageConfig to adjust damage scaling up or down. Damage Factor will not go above 1 or below .1f

Higher level monsters will have their names appear in red. Monsters within your range will be white.

If you are significantly higher level than a monster, your XP award will be reduced. Monsters that are significantly lower level than you will have their names appear in cyan.

All of these formulas functions can be configured in the settings file.
A file listing all monsters and their levels is located in config/EpicMMOSystem/MonsterDB_"Version".jsons

A file called Version.txt is created in the folder. It contains the mod version that was used to create it. Replace it with "NO" to stop it from overwritting on a future update.

Latest Update for Jsons config is 1.5.3 (Number will be updated when Jsons recieve an update)

Please note:
When upgrading the mod to a newer version, new fields in the settings file will be created automatically. You will have to manually re-edit these values if you have changed them.
If you have no custom settings in the configuration file, you should delete the file so that a fresh one can be created by the new version.

Note for other Mods: This mod uses hit.toolTier to pass the Lvl of player



</details>

<details><summary>UI</summary>
	<img src="https://wackymole.com/hosts/MMO_UI.png" width="700"/>

	1HudPanelPosition: Main UI Panel Draggable, default color set by HudBackgroundCol, Type "none" to make it disappear

	HudBarScale: Scale this up or down to resize ALL MMO UI elements. - 1.0 Should cover all of your screen horizontally 

	2-5 UI elements have Position, Scale and Color: 
	 Scale (x, y, z)- z does not matter. - float
	 Color: #(6 digit Hex),  optional 7-8 Digit means alpha. #986100FF (FF -alpha of 1) or use without # red, cyan, blue, 
	 darkblue, lightblue, purple, yellow, lime, fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy, teal, aqua, magenta

	2ExpPanelPosition: Dragable EXP BAR
		To enable ONLY EXP bar , enable eXP Bar Only and restart - not dragable in this mode

	3StaminaPanelPosition: Dragable
	
	4HpPanelPosition: Dragable

	5EitrPanelPosition: Dragable, will disappear and reappear when you have Eitr.

	DisabledHealthIcons: This disables the red Health Icon that is normal present under vanilla health bar

	

</details> 

<details><summary>Console commands</summary>

Admin only commands: - Should work in singleplayer now
 - To set a character's level: `epicmmosystem level [value] [name]` - (If the name contains a space, replace the space with the ampersand (&) symbol)
 - To reset attribute points: `epicmmosystem reset_points [name]` - (If the name contains a space, replace the space with the ampersand (&) symbol)
</details> 

<details><summary>Feedback</summary>

For questions or suggestions please join my discord channel: [Odin Plus Team](https://discord.gg/odinplus)

Support me at https://www.buymeacoffee.com/WackyMole 

<img src="https://wackymole.com/hosts/bmc_qr.png" width="100"/>

Original Creator: LambaSun or my [mod branch](https://discord.com/channels/826573164371902465/977656428670111794)

</details> 

<details>
  <summary><b><span style="color:aqua;font-weight:200;font-size:20px">
    ChangeLog
</span></b></summary>

| Version | Changes                                                                                                                                                                                                                                                                                                                                |
|----------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| 1.5.3: | - Fixed bug in Groups exp sharing. <br/> Added MajesticChickens json
| 1.5.2: | - Added Colors and Scale to Individual UI elements.<br/> Fixed EpicLoot drop bug, made Nav Panel moveable, Eitr UI adjustments<br/> Low_damage_config for extra configurability on low damage mode
| 1.5.1: | - Added Stamina regeneration<br/>
| 1.5.0: | - Changed Config to WackyMole.EpicMMOSystem.cfg<br/> - Made all the UI elements dragable<br/> - Realtime setting of (x,y) position in config, type "none" in BackgroundColor to remove brown bar.<br/> - Added Filewatcher to Jsons<br/> - dedicated Server only<br/> - Added filewatcher to configs, Updated Group logic<br/> - Revamped Mentor mode.<br/>
| 1.4.1: | - Fix Version Check and Multiplayer Sync, moved Monster Bar again.<br/>
| 1.4.0: | - Fix for inventory to bag JC (hopefully)<br/> - Changed Configs,PLEASE DELETE OLD CONFIGS!<br/> - added removeDropMax, removeDropMax,removeBossDropMax, removeBossDropMix, curveExp, curveBossExp.<br/> - Allow for multiple Jsons to be searched<br/> - Added admin rights to singleplayer hosting<br/> - Boss drop is determined by mob.faction(), curveBossExp Exp is just the 6 main bosses. <br/> - Updated Monster.json moved to configs instead of plugin.<br/> - Added ExtraDebugmode for future issues.<br/> - Updated MonserDB_Default for mistlands,LandAnimals mod, MonsterLabZ, Outsiders, SeaAnimals, Fantasy Creatures, Air Animals, and Outsiders.<br/> - Json file in MMO folder is searched.<br/> - Added Version text to easily update in future.<br/> - Write "NO" in Ver.txt to skip future updates. Moved Monster lvl bar [] for boss and non boss<br/>
| 1.3.1: | - Dual wield and EpicMMO Thanks to KG, sponsored by Aldhari/Skaldhari<br/>
| 1.3.0: | - WackyEpicMMOSystem release, until author comes back. Code from Azumatt - Updated Chat, Group and ServerSync<br/>
| 1.2.8: | - Added a limiter for the maximum attribute value.<br/>- New view health and stamina bar (in the configuration you can return the old display where only the experience is displayed).<br/>
| 1.2.7: | - Fix version check<br/>
| 1.2.6: | - Fixed bug of different amount of experience. Added ability to add your own items or currency to reset<br/> attributes.
| 1.2.5: | - Fix damage monsters and fix error for friends list<br/>
| 1.2.4: | - Fix version check<br/>
| 1.2.3: | - Add console command and xp loss on death<br/>
| 1.2.2: | - Add button to open the quest journal (Marketplace) and profession window<br/>
| 1.2.1: | - Fix errors with EAQS<br/>
| 1.2.0: | - Add friends list feature<br/>
| 1.1.0: | - Add creature level control<br/>
| 1.0.1: | - Fix localization and append english text for config comments.<br/>
| 1.0.0: | - Release<br/>
</details> 