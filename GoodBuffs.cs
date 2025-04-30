using System;
using System.Runtime.CompilerServices;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using JetBrains.Annotations;
using MelonLoader;
using StoryTemplate;
using UnityEngine;
using UnityEngine.UIElements;

namespace AscendedBosses
{
    [RegisterTypeInIl2Cpp(false)]
    public class GoodBuffs : MonoBehaviour
    {
        public static GoodBuffs instance;
        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
            }
        }
        public static void CreatePanel()
        {
            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                ModHelperPanel secondPanel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 750, 1850), ModContent.GetTextureGUID<AscendedBosses>("ActualGoodBack"));
                instance = secondPanel.AddComponent<GoodBuffs>();


                ModHelperText GoodText = secondPanel.AddText(new Info("Good_", 0, 625 -100, 500, 250), "BUFFS", 100);
                GoodText.Text.color = Color.yellow;

                ModHelperButton GoodBtn = secondPanel.AddButton(new Info("EvilButton_", 0, -1250 / 2, 600, 200), VanillaSprites.YellowBtnLong, new Action(() => instance.Close()));
                GoodBtn.AddText(new Info("Title_", 0f, 0f, 300, 150), "Ok", 70);

                ModHelperText GoodLine1 = secondPanel.AddText(new Info("LineGood_", 0, 500 - 100, 500, 250), "Towers gains 1 damage", 50);
                ModHelperText GoodLine2 = secondPanel.AddText(new Info("LineGood_", 0, 350 - 100, 500, 250), "Towers gains 5 ranges", 50);
                ModHelperText GoodLine3 = secondPanel.AddText(new Info("LineGood_", 0, 200 - 100, 500, 250), "100 end of round cash slowy increase", 50);
                ModHelperText GoodLine4 = secondPanel.AddText(new Info("LineGood_", 0, 50 - 100, 500, 250), "Cursed Bloons will now spawn", 50);
                GoodLine4.Text.color = Color.red;
                ModHelperText GoodLine5 = secondPanel.AddText(new Info("LineGood_", 0, -100 * 2, 500, 250), "Bloons randomly drop cashs on damage", 50);
                ModHelperText GoodLine6 = secondPanel.AddText(new Info("LineGood_", 0, -250 * 1.5f, 500, 250), "Placed towers have a 5% to get sold", 50);
                GoodLine6.Text.color = Color.red;
                ModHelperText GoodLine7 = secondPanel.AddText(new Info("LineGood_", 0, -400 * 1.2f, 500, 250), "150 life", 50);
            }
        }
    }
}

[HarmonyPatch(typeof(Bloon), nameof(Bloon.PreCheckDamageOutcome))]
public class Bloon_Patch
{
    [HarmonyPostfix]
    public static void Postfix(Bloon __instance)
    {
        System.Random rand = new System.Random();
        if (rand.Next(750) == 0 && AscendedBosses.AscendedBosses.side == "good")
        {
            InGame.instance.AddCash(rand.Next(5000));
        }
        else if (rand.Next(750) == 0 && AscendedBosses.AscendedBosses.side == "evil")
        {
            InGame.instance.AddCash(-rand.Next(2000));
        }
    }
}

[HarmonyPatch(typeof(Bloon), nameof(Bloon.OnSpawn))]
public class BloonSpawn_Patch
{
    [HarmonyPostfix]
    public static void Postfix(Bloon __instance)
    {
        System.Random rand = new System.Random();
        if (rand.Next(200) == 0 && AscendedBosses.AscendedBosses.side == "evil")
        {
            BuffBloonSpeedModel buff = Game.instance.model.GetBloon("Vortex1").GetBehavior<BuffBloonSpeedModel>();
            buff.speedBoost = 1.3f;
            var mutator = buff.Mutator;
            __instance.AddMutator(mutator, 9999);
        }
    }
}

