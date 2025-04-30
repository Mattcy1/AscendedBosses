using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Data.Behaviors.Resources;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.Achievements;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using MelonLoader;
using StoryTemplate;
using UnityEngine;
using UnityEngine.UIElements;

namespace AscendedBosses
{
    [RegisterTypeInIl2Cpp(false)]
    public class BossChoose : MonoBehaviour
    {
        public static BossChoose instance;
        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
            }
        }
        public static void CreatePanel(int r)
        {
            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 0, 2000, 1500), ModContent.GetTextureGUID<AscendedBosses>("ActualGoodBack"));
                instance = panel.AddComponent<BossChoose>();
                ModHelperText BossText = panel.AddText(new Info("BossText", -300, 500, 500, 250), "Choose", 100);
                ModHelperText BossText1 = panel.AddText(new Info("BossText", 400, 500, 500, 250), $"R{r} Boss", 100);
                if (r == 40)
                {
                    ModHelperButton BossBtn = panel.AddButton(new Info("BossBtn1", -500, -100, 750, 1000), VanillaSprites.YellowBtnSquare, new Action(() => ChooseBoss("Vortex", r)));
                    ModHelperButton BossBtn1 = panel.AddButton(new Info("BossBtn1", 500, -100, 750, 1000), VanillaSprites.YellowBtnSquare, new Action(() => ChooseBoss("Bloonarius", r)));
                    ModHelperImage BossBtnImg = BossBtn.AddImage(new Info("BossBtnImg", 0, 0, 700, 1000), ModContent.GetTextureGUID<AscendedBosses>("AscendedVortex-Icon"));
                    ModHelperImage BossBtn1Img = BossBtn1.AddImage(new Info("BossBtn1Img", 0, 0, 700, 1000), ModContent.GetTextureGUID<AscendedBosses>("AscendedBloonarius-Icon"));
                }
                else if (r == 60)
                {
                    ModHelperButton BossBtn = panel.AddButton(new Info("BossBtn1", -500, -100, 750, 1000), VanillaSprites.YellowBtnSquare, new Action(() => ChooseBoss("Phayze", r)));
                    ModHelperButton BossBtn1 = panel.AddButton(new Info("BossBtn1", 500, -100, 750, 1000), VanillaSprites.YellowBtnSquare, new Action(() => ChooseBoss("Lych", r)));
                    ModHelperImage BossBtnImg = BossBtn.AddImage(new Info("BossBtnImg", 0, 0, 700, 1000), ModContent.GetTextureGUID<AscendedBosses>("AscendedPhayze-Icon"));
                    ModHelperImage BossBtn1Img = BossBtn1.AddImage(new Info("BossBtn1Img", 0, 0, 700, 1000), ModContent.GetTextureGUID<AscendedBosses>("AscendedLych-Icon"));
                }
                else if (r == 80)
                {
                    ModHelperButton BossBtn = panel.AddButton(new Info("BossBtn1", -500, -100, 750, 1000), VanillaSprites.YellowBtnSquare, new Action(() => ChooseBoss("Dreadbloon", r)));
                    ModHelperButton BossBtn1 = panel.AddButton(new Info("BossBtn1", 500, -100, 750, 1000), VanillaSprites.YellowBtnSquare, new Action(() => ChooseBoss("Blast", r)));
                    ModHelperImage BossBtnImg = BossBtn.AddImage(new Info("BossBtnImg", 0, 0, 700, 1000), ModContent.GetTextureGUID<AscendedBosses>("AscendedBlast-Icon"));
                    ModHelperImage BossBtn1Img = BossBtn1.AddImage(new Info("BossBtn1Img", 0, 0, 700, 1000), ModContent.GetTextureGUID<AscendedBosses>("AscendedDreadbloon-Icon"));
                }
            }
        }

        public static void ChooseBoss(string b, int r)
        {
            if(r == 40)
            {
                AscendedBosses.Bosses[0] = b;
                instance.Close();
                CreatePanel(60);
            }
            if (r == 60)
            {
                AscendedBosses.Bosses[1] = b;
                instance.Close();
                CreatePanel(80);
            }
            if (r == 80)
            {
                AscendedBosses.Bosses[2] = b;
                instance.Close();
            }

            foreach (var boss in AscendedBosses.Bosses)
            {
                MelonLogger.Msg(boss);
            }
        }
    }
}

