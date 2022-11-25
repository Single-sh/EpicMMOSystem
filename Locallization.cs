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
        else if (currentLanguage == "Spanish")
        {
            SpanLocalization();
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
        _dictionary.Add("$magic_damage", "Elemental damage");
        _dictionary.Add("$magic_armor", "Elemental reduced");
        _dictionary.Add("$add_hp", "Health increase");
        _dictionary.Add("$add_stamina", "Stamina increase");
        _dictionary.Add("$physic_armor", "Physical reduced");
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

    private void SpanLocalization()
    {
        _dictionary.Add("$attributes", "Atributos");
        _dictionary.Add("$parameter_strength", "Fuerza");
        _dictionary.Add("$parameter_agility", "Agilidad");
        _dictionary.Add("$parameter_intellect", "Intelecto");
        _dictionary.Add("$parameter_body", "Resistencia");
        _dictionary.Add("$free_points", "Puntos disponibles");
        _dictionary.Add("$level", "Nivel");
        _dictionary.Add("$lvl", "Nivel.");
        _dictionary.Add("$exp", "Experiencia");
        _dictionary.Add("$cancel", "Cancelar");
        _dictionary.Add("$apply", "Aceptar");
        _dictionary.Add("$reset_parameters", "Restablecer puntos");
        _dictionary.Add("$no", "No");
        _dictionary.Add("$yes", "Si");
        _dictionary.Add("$get_exp", "Experiencia recibida");
        _dictionary.Add("$reset_point_text", "¿De verdad quieres eliminar todos los puntos por {0} {1}?");
        //Parameter
        _dictionary.Add("$physic_damage", "Daño Físico");
        _dictionary.Add("$add_weight", "Cargar Peso");
        _dictionary.Add("$speed_attack", "Consumo de resistencia de ataque");
        _dictionary.Add("$reduced_stamina", "Consumo de energía (correr, saltar)");
        _dictionary.Add("$magic_damage", "Daño mágico");
        _dictionary.Add("$magic_armor", "Armadura mágica");
        _dictionary.Add("$add_hp", "Aumento de la salud");
        _dictionary.Add("$add_stamina", "Aumento de resistencia");
        _dictionary.Add("$physic_armor", "Armadura física");
        _dictionary.Add("$reduced_stamina_block", "Consumo de energia de bloqueo");
        _dictionary.Add("$regen_hp", "Regeneración de salud");
        _dictionary.Add("$damage", "Daño");
        _dictionary.Add("$armor", "Armadura");
        _dictionary.Add("$survival", "Surpervivencia");
        //Friends list
        _dictionary.Add("$notify", "<color=#00E6FF>Alerta</color>");
        _dictionary.Add("$friends_list", "Lista de amigos");
        _dictionary.Add("$send", "Enviar");
        _dictionary.Add("$invited", "Invitaciones");
        _dictionary.Add("$friends", "Amigos");
        _dictionary.Add("$online", "Conectado");
        _dictionary.Add("$offline", "Desconectado");
        _dictionary.Add("$not_found", "Player {0} inexistente.");
        _dictionary.Add("$send_invite", "Se ha enviado una solicitud de amistad a {0}.");
        _dictionary.Add("$get_invite", "Has recivido una solicitud de amistad de {0}.");
        _dictionary.Add("$accept_invite", "{0} aceptó la solicitud de amistad.");
        _dictionary.Add("$cancel_invite", "{0} denegó la solicitud de amistad.");
        //Terminal
        _dictionary.Add("$terminal_set_level", "Eres nivel {0}");
        _dictionary.Add("$terminal_reset_points", "Tus puntos de atributos han sido reiniciados");
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