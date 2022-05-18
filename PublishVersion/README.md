﻿# Epic MOO System
Adds to your Valheim world a system of levels and distribution of characteristics:
![https://i.imgur.com/iA4JRuM.png](https://i.imgur.com/iA4JRuM.png)

Features:
 - Works with group mods. You can gain experience together playing in a group. Without a group, the experience goes to the one who last hit the monster.
 - You can add your own monsters to the list in order to get experience from them.
 - Experience progress bar on screen
 - Compatible with KGMarketplace mod, you can add to the reward (EpicMMO_Exp: text, 250) gain experience or limit the quest by level (EpicMMO_Level: text, 20).
 - Compatible with ItemRequiresSkillLevel, you can now limit items by level or attribute.
 ![https://i.imgur.com/lkCcVOo.png](https://i.imgur.com/lkCcVOo.png)
 
<details><summary>Creature level control:</summary>

Monster added level.
![https://i.imgur.com/IySsj3j.png](https://i.imgur.com/IySsj3j.png)
You'll get nothing (exp, drop) if monster is higher than your level (your level + additional level from configs), also damage on monster will be lower by multiplied you/monster level percentage (20/50 = 0.4, you'll deal 0.4% of your damage). Such monsters will have a red name.
If you are much stronger than the monster in level, then you will get less experience. Such monsters will have a cyan name color.

All functions can be configured in the configuration.
Also a file with all monsters and level is located in plugin/EpicMMOSystem/MonsterDB_"Version".

Attention!
If you had a previous version of the file, then new fields will be created automatically which you will have to correct for your necessary values. 
If you didn't do anything then just delete file and it will create new one with default values.

</details>
<details><summary>Changelog:</summary>

 - 1.1.0: Add creature level control
 - 1.0.1: Fix localization and append english text for config comments.
 - 1.0.0: Release
</details> 