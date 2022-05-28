﻿# Description
Adds to your Valheim world a system of levels and distribution of characteristics:
![https://i.imgur.com/iA4JRuM.png](https://i.imgur.com/iA4JRuM.png)

Features:
 - Works with group mods. You can gain experience together playing in a group. Without a group, the experience goes to the one who last hit the monster.
 - You can add your own monsters to the list in order to get experience from them.
 - Friends List.
 - Experience progress bar on screen.
 - Compatible with ItemRequiresSkillLevel, you can now limit items by level or attribute.
 - Compatible with KGMarketplace mod, you can add to the reward (EpicMMO_Exp: text, 250) gain experience or limit the quest by level (EpicMMO_Level: text, 20).
 
 ![https://i.imgur.com/lkCcVOo.png](https://i.imgur.com/lkCcVOo.png)

<details><summary>Friends list</summary>

Click the plus button at the bottom of the friends bar. Enter your friend's name with a capital letter and send an invitation to become friends.
   ![https://i.imgur.com/rC8RDYe.png](https://i.imgur.com/rC8RDYe.png)
Your friend will receive an invitation. Once accepted, it will appear in your list of friends. You can invite him to the group from the friends panel by clicking the button with the people. 
   ![https://i.imgur.com/W460hdu.png](https://i.imgur.com/W460hdu.png)

# Warning. 
- If your friend is out of the game and you just accepted the invitation then friend will not receive a notification and you will not be added to his list of friends. You will need to re-send an invitation.
- You can not send an invitation to yourself or a friend you already have in your friends. Remove him from your friends list and then send him an invitation if necessary.
- Invitations to friends is not saved after leaving. Accept them before leaving the game. 
</details> 

<details><summary>Creature level control</summary>

Monsters have been added a level.
![https://i.imgur.com/IySsj3j.png](https://i.imgur.com/IySsj3j.png)
You'll get nothing (exp, drop) if monster is higher than your level (your level + additional level from configs), also damage on monster will be lower by multiplied you/monster level percentage (20/50 = 0.4, you'll deal 0.4% of your damage). Such monsters will have a red name.
If you are much stronger than the monster in level, then you will get less experience. Such monsters will have a cyan name color.

All functions can be configured in the configuration.
Also a file with all monsters and level is located in plugin/EpicMMOSystem/MonsterDB_"Version".

Attention!
If you had a previous version of the file, then new fields will be created automatically which you will have to correct for your necessary values. 
If you didn't do anything then just delete file and it will create new one with default values.

</details>


<details><summary>Console commands</summary>

Commands only admin.
 - Set level: epicmmosystem level [value] [name] - if name have space change space on &.
 - Reset points attribute: epicmmosystem reset_points [name] - if name have space change space on &.
</details> 

<details><summary>Feedback</summary>

You can ask questions or suggest ideas in the discord channel [Odin Plus Team](https://discord.gg/uf44CtCm), look for me there under the nickname LambaSun or my [mod branch](https://discord.com/channels/826573164371902465/977656428670111794).
</details> 

<details><summary>Changelog</summary>
 
 - 1.2.3: Add console command and loss exp if dead
 - 1.2.2: Add button for open quest journal (Marketplace) and profession
 - 1.2.1: Fix errors with EAQS
 - 1.2.0: Add friends list
 - 1.1.0: Add creature level control
 - 1.0.1: Fix localization and append english text for config comments.
 - 1.0.0: Release
</details> 