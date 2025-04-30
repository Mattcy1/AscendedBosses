using System;
using System.Collections;
using System.Collections.Generic;
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
using Il2CppAssets.Scripts.SimulationTests;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppSystem.Threading;
using JetBrains.Annotations;
using MelonLoader;
using StoryTemplate;
using UnityEngine;
using UnityEngine.UIElements;
using static StoryTemplate.Story;

namespace AscendedBosses
{
    [RegisterTypeInIl2Cpp(false)]
    public class BossUI : MonoBehaviour
    {
        public static BossUI instance;
        public static ModHelperPanel bossPanel;
        public static ModHelperImage bossPanelInside;
        public static ModHelperImage bossLeftBackground;
        public static ModHelperImage bossStars;
        public static ModHelperText bossHealth;
        public static ModHelperImage bossIcon;
        public static ModHelperText bossName;
        public static List<ModHelperImage> skulls = new List<ModHelperImage>();
        public void Close()
        {
            if (gameObject)
            {
                gameObject.Destroy();
            }
        }
        public static void CreatePanel(Bloon b)
        {
            if (InGame.instance != null)
            {
                RectTransform rect = InGame.instance.uiRect;
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0, 1150, 1250, 100), ModContent.GetTextureGUID<AscendedBosses>("HealthbarBG"));
                instance = panel.AddComponent<BossUI>();
                ModHelperImage panelInside = panel.AddImage(new Info("PanelInside", 0, 0, 1250, 90), ModContent.GetTextureGUID<AscendedBosses>("Healthbar"));
                ModHelperImage letfBackground = panel.AddImage(new Info("LeftBackground", -725, 0f, 250, 250), VanillaSprites.BossTiersIconSmall);
                ModHelperImage stars = panel.AddImage(new Info("stars", -475, 100, 300, 100), ModContent.GetTextureGUID<AscendedBosses>("Tier5Boss"));
                ModHelperText Health = panel.AddText(new Info("HealthText_", 450, 80, 500, 250), b.health + "/" + b.bloonModel.maxHealth, 50);
                ModHelperImage Icon = letfBackground.AddImage(new Info("leftIcon", 0, 0f, 250, 250), VanillaSprites.BloonariusIcon);
                ModHelperText Name = panel.AddText(new Info("NameText_", 0, 80, 750, 250), "Bloonarius", 50);
                bossName = Name;
                bossPanel = panel;
                bossPanelInside = panelInside;
                bossLeftBackground = letfBackground;
                bossStars = stars;
                bossHealth = Health;
                bossIcon = Icon;

                if (b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedPhayze>())
                {
                    CreateSkulls(3, panel);
                }
                else if (b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedDread>())
                {
                    CreateSkulls(5, panel);
                }
                else
                {
                    CreateSkulls(10, panel);
                }
                HandleUI(b);
            }
        }

        public static void CreateSkulls(int number, ModHelperPanel panel)
        {
            float panelWidth = panel.RectTransform.rect.width; 
            float spacing = panelWidth / (number + 1); 

            for (int i = 0; i < number; i++)
            {
                float xPosition = (i + 1) * spacing; 

                ModHelperImage skull = panel.AddImage(new Info($"Skull_{i}", xPosition - panelWidth / 2, 0,  175, 175), ModContent.GetTextureGUID<AscendedBosses>("Skull"));

                skull.RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                skull.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                skull.RectTransform.anchoredPosition = new Vector2(xPosition - panelWidth / 2, -panel.RectTransform.rect.height / 3);
                skulls.Add(skull);
            }
        }

        public static void HandleUI(Bloon b)
        {
            if(instance != null && b != null)
            {
                if(b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBloonarius>())
                {
                    bossName.Text.text = "Ascended Bloonarius";
                    bossIcon.Destroy();
                    ModHelperImage Icon = bossLeftBackground.AddImage(new Info("leftIcon", 0, 0f, 200, 200), ModContent.GetTextureGUID<AscendedBosses>("AscendedBloonarius-Icon"));
                    bossIcon = Icon;
                }
                else if(b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedVortex>())
                {
                    bossName.Text.text = "Ascended Vortex";
                    bossIcon.Destroy();
                    ModHelperImage Icon = bossLeftBackground.AddImage(new Info("leftIcon", 0, 0f, 200, 200), ModContent.GetTextureGUID<AscendedBosses>("AscendedVortex-Icon"));
                    bossIcon = Icon;
                }
                else if (b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedLych>())
                {
                    bossName.Text.text = "Ascended Lych";
                    bossIcon.Destroy();
                    ModHelperImage Icon = bossLeftBackground.AddImage(new Info("leftIcon", 0, 0f, 200, 200), ModContent.GetTextureGUID<AscendedBosses>("AscendedLych-Icon"));
                    bossIcon = Icon;
                }
                else if (b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedPhayze>())
                {
                    bossIcon.Destroy();
                    ModHelperImage Icon = bossLeftBackground.AddImage(new Info("leftIcon", 0, 0f, 200, 200), ModContent.GetTextureGUID<AscendedBosses>("AscendedPhayze-Icon"));
                    bossIcon = Icon;
                }
                else if (b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBlast>())
                {
                    bossName.Text.text = "Ascended Blast";
                    bossIcon.Destroy();
                    ModHelperImage Icon = bossLeftBackground.AddImage(new Info("leftIcon", 0, 0f, 200, 200), ModContent.GetTextureGUID<AscendedBosses>("AscendedBlast-Icon"));
                    bossIcon = Icon;
                }
                else if (b.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedDread>())
                {
                    bossName.Text.text = "Ascended Dread";
                    bossIcon.Destroy();
                    ModHelperImage Icon = bossLeftBackground.AddImage(new Info("leftIcon", 0, 0f, 200, 200), ModContent.GetTextureGUID<AscendedBosses>("AscendedDreadbloon-Icon"));
                    bossIcon = Icon;
                }
                bossHealth.Text.text = b.health + "/" + b.bloonModel.maxHealth;

                float healthPercentage = (float)b.health / b.bloonModel.maxHealth;

                RectTransform panelTransform = bossPanelInside.GetComponent<RectTransform>();

                panelTransform.anchorMin = new Vector2(0, 0.5f);
                panelTransform.anchorMax = new Vector2(0, 0.5f);
                panelTransform.pivot = new Vector2(0, 0.5f);

                float originalWidth = 1250;
                float newWidth = originalWidth * healthPercentage;
                panelTransform.sizeDelta = new Vector2(originalWidth * healthPercentage, panelTransform.sizeDelta.y);

                for (int i = skulls.Count - 1; i >= 0; i--)
                {
                    if (skulls[i].RectTransform.anchoredPosition.x >= newWidth - (originalWidth / 2))
                    {
                        MelonCoroutines.Start(RemoveSkullWithEffect(skulls[i]));
                    }
                }
            }
            else if(b == null)
            {
                instance.Close();
            }
        }

        public static IEnumerator RemoveSkullWithEffect(ModHelperImage skull)
        {
            skull.AddImage(new Info($"Skull_", 0, 0, 175, 175), ModContent.GetTextureGUID<AscendedBosses>("ActivateSkull"));

            yield return new WaitForSeconds(1f);

            if (skull != null && skull.gameObject != null)
            {
                GameObject.Destroy(skull.gameObject);
                skulls.Remove(skull);
            }
        }
    }
}

