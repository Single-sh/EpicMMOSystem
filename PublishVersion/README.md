﻿# Description
This mod adds an RPG-like system of levels and attribute increases:
![https://i.imgur.com/5Tzgs0R.png](https://i.imgur.com/5Tzgs0R.png)

Features:
 - Shared group XP. Outside of groups all XP awards go to the character who struck the last blow.
 - Custom mobs can be added for XP gain.
 - MMO-like friends list.
 - On screen XP bar.
 - Compatible with [ItemRequiresSkillLevel](https://valheim.thunderstore.io/package/Detalhes/ItemRequiresSkillLevel/) mod. Equipment can be limited by level or attribute.
 - Compatible with KGMarketplace mod. Experience rewards can be added: (EpicMMO_Exp: text, 250) Quests can be limited by level (EpicMMO_Level: text, 20).
 
 ![https://i.imgur.com/lkCcVOo.png](https://i.imgur.com/lkCcVOo.png)

<details><summary>Friends list</summary>

Click the plus button at the bottom of the friends bar. Enter the name of the character you wish to add, starting with a capital letter.
   ![https://i.imgur.com/rC8RDYe.png](https://i.imgur.com/rC8RDYe.png)
The player will receive a friend request. Once accepted, it the character will appear in your friends list. Group invites can be sent from the friends list. 
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

</details>


<details><summary>Console commands</summary>

Admin only commands:
 - To set a character's level: `epicmmosystem level [value] [name]` - (If the name contains a space, replace the space with the ampersand (&) symbol)
 - To reset attribute points: `epicmmosystem reset_points [name]` - (If the name contains a space, replace the space with the ampersand (&) symbol)
</details> 

<details><summary>Feedback</summary>

For questions or suggestions please join my discord channel: [Odin Plus Team](https://discord.gg/uf44CtCm)
Discord nickname: LambaSun or my [mod branch](https://discord.com/channels/826573164371902465/977656428670111794)
</details> 

<details><summary>Changelog</summary>
 
 - 1.2.4: Fix version check
 - 1.2.3: Add console command and xp loss on death
 - 1.2.2: Add button to open the quest journal (Marketplace) and profession window
 - 1.2.1: Fix errors with EAQS
 - 1.2.0: Add friends list feature
 - 1.1.0: Add creature level control
 - 1.0.1: Fix localization and append english text for config comments.
 - 1.0.0: Release
</details> 