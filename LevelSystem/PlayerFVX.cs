using System;
using HarmonyLib;
using UnityEngine;

namespace EpicMMOSystem;

public static class PlayerFVX
{

    public static void levelUp()
    {
        Transform parent = Player.m_localPlayer.transform.Find("Visual/Armature/Hips/Spine/Spine1/Spine2");
        var vfx = EpicMMOSystem.Instantiate(ZNetScene.instance.GetPrefab("LevelUpVFX"), parent).transform;
        vfx.localPosition = Vector3.zero;
        vfx.localScale = new Vector3(0.01352632f, 0.01352632f, 0.01352632f);
    }

    // [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))]
    // public static class UpdateExpPanelForStart
    // {
    //     static void Postfix()
    //     {
    //         GameObject prefab = EpicMmoSystemPlugin._asset.LoadAsset<GameObject>("LevelUpVFX");
    //         Transform parent = Player.m_localPlayer.transform.Find("Visual/Armature/Hips/Spine/Spine1/Spine2");
    //         levelUpVFX = EpicMmoSystemPlugin.Instantiate(prefab, parent).transform;
    //         levelUpVFX.localPosition = Vector3.zero;
    //         levelUpVFX.localScale = new Vector3(0.01552632f, 0.01552632f, 0.01552632f);
    //     }
    // }
}