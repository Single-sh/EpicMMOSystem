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
    private string defaultFileName = "rus_emmosLocalization.txt";
    private Dictionary<string, string> _dictionary = new ();

    public Localization()
    {
        var fileName = $"{EpicMMOSystem.language.Value}_emmosLocalization.txt";
        var basePath = Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, fileName);
        if (File.Exists(basePath))
        {
            ReadLocalization(basePath);
            return;
        }
        CreateLocalizationFile(basePath);
    }

    private void ReadLocalization(string path)
    {
        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            var pair = line.Split('=');
            var text = pair[1].Replace('*', '\n');
            _dictionary.Add(pair[0].Trim(), text.TrimStart());
        }
    }

    private void CreateLocalizationFile(string path)
    {
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
        _dictionary.Add("$reset_point_text", "Вы действительно хотите сбросить все поинты за {0} золотых ?");
        //Parameter
        _dictionary.Add("$physic_damage", "Ув. физ. урона");
        _dictionary.Add("$add_weight", "Ув. переносимого веса");
        _dictionary.Add("$speed_attack", "Расход. вын. на атаку");
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
        _dictionary.Add("$survarior", "Выживание");
        
        
        List<string> list = new List<string>();
        foreach (var pair in _dictionary)
        {
            list.Add($"{pair.Key} = {pair.Value}");
        }
        DirectoryInfo dir = new DirectoryInfo(Paths.PluginPath);
        dir.CreateSubdirectory(Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName));
        File.WriteAllLines(Path.Combine(Paths.PluginPath, EpicMMOSystem.ModName, defaultFileName), list);
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