using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using BepInEx;
using UnityEngine;

namespace EpicMMOSystem;

public class Localization
{
    private string defaultFileName = "eng_emmosLocalization.txt";
    private Dictionary<string, string> _dictionary = new ();

    public Localization()
    {
        var currentLanguage = global::Localization.instance.GetSelectedLanguage();
        if (currentLanguage == "Russian")
        {
            RusLocalization();
        }
        else if (currentLanguage == "English")
        {
            EngLocalization();
        }
        else
        {
            var fileName = $"{EpicMMOSystem.language.Value}_emmosLocalization.txt";
            var basePath = Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, fileName);
            if (File.Exists(basePath))
            {
                ReadLocalization(basePath);
                return;
            }
            CreateLocalizationFile();
        }
    }

    private void ReadLocalization(string path)
    {
        var lines = File.ReadAllLines(path);
        EngLocalization();
        bool update = _dictionary.Count > lines.Length;
        foreach (var line in lines)
        {
            var pair = line.Split('=');
            var text = pair[1].Replace('*', '\n');
            _dictionary[pair[0].Trim()] = text.TrimStart();
        }
        if (update)
        {
            List<string> list = new List<string>();
            foreach (var pair in _dictionary)
            {
                list.Add($"{pair.Key} = {pair.Value}");
            }
            File.WriteAllLines(path, list);
        }
    }

    private void CreateLocalizationFile()
    {
        EngLocalization();
        List<string> list = new List<string>();
        foreach (var pair in _dictionary)
        {
            list.Add($"{pair.Key} = {pair.Value}");
        }
        DirectoryInfo dir = new DirectoryInfo(Paths.PluginPath);
        dir.CreateSubdirectory(Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName));
        File.WriteAllLines(Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, defaultFileName), list);
    }

    private void RusLocalization()
    {
        _dictionary.Add("$attributes", "Параметры");
        _dictionary.Add("$parameter_strength", "Сила");
        _dictionary.Add("$parameter_agility", "Ловкость");
        _dictionary.Add("$parameter_intellect", "Интеллект");
        _dictionary.Add("$parameter_body", "Телосложение");
        _dictionary.Add("$free_points", "Доступно очков");
        _dictionary.Add("$level", "Уровень");
        _dictionary.Add("$lvl", "Ур.");
        _dictionary.Add("$exp", "Опыт");
        _dictionary.Add("$cancel", "Отмена");
        _dictionary.Add("$apply", "Принять");
        _dictionary.Add("$reset_parameters", "Сбросить параметры");
        _dictionary.Add("$no", "Нет");
        _dictionary.Add("$yes", "Да");
        _dictionary.Add("$get_exp", "Получено опыта");
        _dictionary.Add("$reset_point_text", "Вы действительно хотите сбросить все поинты за {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Ув. физ. урона");
        _dictionary.Add("$add_weight", "Ув. переносимого веса");
        _dictionary.Add("$speed_attack", "Расход вын. на атаку");
        _dictionary.Add("$reduced_stamina", "Расход вын. (бег, прыжок)");
        _dictionary.Add("$magic_damage", "Ув. маг. урона");
        _dictionary.Add("$magic_armor", "Ув. маг. защиты");
        _dictionary.Add("$add_hp", "Ув. здоровья");
        _dictionary.Add("$add_stamina", "Ув. выносливости");
        _dictionary.Add("$physic_armor", "Ув. физ. защиты");
        _dictionary.Add("$reduced_stamina_block", "Расход вын. на блок");
        _dictionary.Add("$regen_hp", "Регенерация здоровья");
        _dictionary.Add("$damage", "Урон");
        _dictionary.Add("$armor", "Защита");
        _dictionary.Add("$survival", "Выживание");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Оповещение</color>");
        _dictionary.Add("$friends_list", "Список друзей");
        _dictionary.Add("$send", "Отправить");
        _dictionary.Add("$invited", "Приглашения");
        _dictionary.Add("$friends", "Друзья");
        _dictionary.Add("$online", "В игре");
        _dictionary.Add("$offline", "Нет в игре");
        _dictionary.Add("$not_found", "Игрок {0} не найден.");
        _dictionary.Add("$send_invite", "Игроку {0}, отправлен запрос в друзья.");
        _dictionary.Add("$get_invite", "Получен запрос в друзья от {0}.");
        _dictionary.Add("$accept_invite", "Игрок {0}, принял запрос в друзья.");
        _dictionary.Add("$cancel_invite", "Игрок {0}, отменил запрос в друзья.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Вы получили {0} уровень");
        _dictionary.Add("$terminal_reset_points", "Ваши очки характеристик были сброшены");
    }
    private void EngLocalization()
    {
        _dictionary.Add("$attributes", "Attributes");
        _dictionary.Add("$parameter_strength", "Strength");
        _dictionary.Add("$parameter_agility", "Agility");
        _dictionary.Add("$parameter_intellect", "Intellect");
        _dictionary.Add("$parameter_body", "Endurance");
        _dictionary.Add("$free_points", "Available points");
        _dictionary.Add("$level", "Level");
        _dictionary.Add("$lvl", "Lvl.");
        _dictionary.Add("$exp", "Experience");
        _dictionary.Add("$cancel", "Cancel");
        _dictionary.Add("$apply", "Accept");
        _dictionary.Add("$reset_parameters", "Reset points");
        _dictionary.Add("$no", "No");
        _dictionary.Add("$yes", "Yes");
        _dictionary.Add("$get_exp", "Experience received");
        _dictionary.Add("$reset_point_text", "Do you really want to drop all the points for {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Physical Damage");
        _dictionary.Add("$add_weight", "Carry weight");
        _dictionary.Add("$speed_attack", "Attack stamina consumption");
        _dictionary.Add("$reduced_stamina", "Stamina consumption (running, jumping)");
        _dictionary.Add("$magic_damage", "Magic damage");
        _dictionary.Add("$magic_armor", "Magic armor");
        _dictionary.Add("$add_hp", "Health increase");
        _dictionary.Add("$add_stamina", "Stamina increase");
        _dictionary.Add("$physic_armor", "Physical armor");
        _dictionary.Add("$reduced_stamina_block", "Block stamina consumption");
        _dictionary.Add("$regen_hp", "Health regeneration");
        _dictionary.Add("$damage", "Damage");
        _dictionary.Add("$armor", "Armor");
        _dictionary.Add("$survival", "Survival");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Alert</color>");
        _dictionary.Add("$friends_list", "Friends list");
        _dictionary.Add("$send", "Send");
        _dictionary.Add("$invited", "Invitations");
        _dictionary.Add("$friends", "Friends");
        _dictionary.Add("$online", "Online");
        _dictionary.Add("$offline", "Offline");
        _dictionary.Add("$not_found", "Player {0} is not found.");
        _dictionary.Add("$send_invite", "A friend request has been sent to player {0}.");
        _dictionary.Add("$get_invite", "Received a friend request from {0}.");
        _dictionary.Add("$accept_invite", "Player {0}, accepted the friend request.");
        _dictionary.Add("$cancel_invite", "Player {0}, canceled his friend request.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "You got {0} level");
        _dictionary.Add("$terminal_reset_points", "Your attributes points have been reset");
    }
    
    public string this[string key]
    {
        get
        {
            if (_dictionary.ContainsKey(key))
            {
                return _dictionary[key];
            }
            return "Missing language key";
        }
    }
}