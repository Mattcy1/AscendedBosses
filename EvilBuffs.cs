using System;
using System.Runtime.CompilerServices;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
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
    public class EvilBuffs : MonoBehaviour
    {
        public static EvilBuffs instance;
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
                ModHelperPanel secondPanel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 650, 1750), ModContent.GetTextureGUID<AscendedBosses>("EvilBack"));
                instance = secondPanel.AddComponent<EvilBuffs>();
                ModHelperImage panelInside = secondPanel.AddImage(new Info("Image_", 0, 0, 750, 2000), ModContent.GetTextureGUID<AscendedBosses>("EvilFront"));


                ModHelperText GoodText = secondPanel.AddText(new Info("Good_", 0, 625, 500, 250), "BUFFS", 100);
                GoodText.Text.color = Color.red;

                ModHelperButton GoodBtn = secondPanel.AddButton(new Info("EvilButton_", 0, -1250 / 2, 600, 200), VanillaSprites.RedBtnLong, new Action(() => instance.Close()));
                GoodBtn.AddText(new Info("Title_", 0f, 0f, 300, 150), "Ok", 70);

                ModHelperText GoodLine1 = secondPanel.AddText(new Info("LineGood_", 0, 500, 500, 250), "Towers loses 5 range", 50);
                ModHelperText GoodLine2 = secondPanel.AddText(new Info("LineGood_", 0, 350, 500, 250), "Bloons steals your money", 50);
                ModHelperText GoodLine3 = secondPanel.AddText(new Info("LineGood_", 0, 200, 500, 250), "Bloons Randomly Gets Speed Buff", 50);
                ModHelperText GoodLine4 = secondPanel.AddText(new Info("LineGood_", 0, 50, 500, 250), "Cursed Bloons will now spawn", 50);
                ModHelperText GoodLine5 = secondPanel.AddText(new Info("LineGood_", 0, -100, 500, 250), "2 Random Towers will get blocked", 50);
                ModHelperText GoodLine6 = secondPanel.AddText(new Info("LineGood_", 0, -250, 500, 250), "1 life", 50);
                ModHelperText GoodLine7 = secondPanel.AddText(new Info("LineGood_", 0, -400, 500, 250), "Gain 1K Every 10 Round", 50);
                GoodLine1.Text.color = Color.red;
                GoodLine2.Text.color = Color.red;
                GoodLine3.Text.color = Color.red;
                GoodLine4.Text.color = Color.red;
                GoodLine5.Text.color = Color.red;
                GoodLine6.Text.color = Color.red;
            }
        }
    }
}


