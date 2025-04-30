using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts.Data.Boss;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Simulation.Bloons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.InGame.RightMenu.Powers;
using Il2CppSystem.Threading;
using JetBrains.Annotations;
using MelonLoader;
using StoryTemplate;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using static MelonLoader.MelonLogger;
using static StoryTemplate.Story;

namespace AscendedBosses
{
    [RegisterTypeInIl2Cpp(false)]
    public class FinalBossUI : MonoBehaviour
    {
        public static ModHelperText Sunhealth;
        public static ModHelperText Evilhealth;
        public static ModHelperImage SunInside;
        public static ModHelperImage EvilInside;
        public static FinalBossUI instance;
        public static UnityEngine.Vector2 FianlBoss1Pos = new UnityEngine.Vector2(999, 999);
        public static UnityEngine.Vector2 FianlBossPos = new UnityEngine.Vector2(9999, 9999);
        public static float sunFhealth = 10000000;
        public static float evilFHealth = 10000000;
        public static bool einfHp;
        public static bool sinfHp;
        public static List<ModHelperImage> skulls = new List<ModHelperImage>();
        public static Bloon eboss;
        public static Bloon sboss;
        public static bool Return = false;
        public static bool ReturnEvil = false;
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
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new("EvilPanel", 0, 1150, 1250, 100), ModContent.GetTextureGUID<AscendedBosses>("HealthbarBG"));
                instance = panel.AddComponent<FinalBossUI>();
                ModHelperImage panelInside = panel.AddImage(new Info("PanelInside", 0, 0, 1250, 90), ModContent.GetTextureGUID<AscendedBosses>("Evilbar"));
                ModHelperImage letfBackground = panel.AddImage(new Info("LeftBackground", -725, 0f, 250, 250), VanillaSprites.BossTiersIconSmall);
                ModHelperImage stars = panel.AddImage(new Info("stars", -475, 100, 300, 100), ModContent.GetTextureGUID<AscendedBosses>("Tier5Boss"));
                ModHelperText Health = panel.AddText(new Info("HealthText_", 450, 80, 500, 250), "", 50);
                ModHelperImage Icon = letfBackground.AddImage(new Info("leftIcon", 0, 0f, 250, 250), ModContent.GetTextureGUID<AscendedBosses>("AsmodeusBloon"));
                ModHelperText Name = panel.AddText(new Info("NameText_", 0, 80, 750, 250), "Asmodeus", 50);


                ModHelperImage SunPanel = panel.AddImage(new("SunPanel", 0, -200, 1250, 100), ModContent.GetTextureGUID<AscendedBosses>("HealthbarBG"));
                ModHelperImage SunpanelInside = SunPanel.AddImage(new Info("PanelInside", 0, 0, 1250, 90), ModContent.GetTextureGUID<AscendedBosses>("Sunbar"));
                ModHelperImage SunletfBackground = SunPanel.AddImage(new Info("LeftBackground", 725, 0f, 250, 250), VanillaSprites.BossTiersIconSmall);
                ModHelperImage Sunstars = SunPanel.AddImage(new Info("stars", 450, 80, 300, 100), ModContent.GetTextureGUID<AscendedBosses>("Tier5Boss"));
                ModHelperText SunHealth = SunPanel.AddText(new Info("HealthText_", -475, 100, 500, 250), "", 50);
                ModHelperImage SunIcon = SunletfBackground.AddImage(new Info("leftIcon",0, 0f, 250, 250), ModContent.GetTextureGUID<AscendedBosses>("HeliosBloonIcon"));
                ModHelperText SunName = SunPanel.AddText(new Info("NameText_", 0, 80, 750, 250), "Helios", 50);

                SunInside = SunpanelInside;
                EvilInside = panelInside;
                Sunhealth = SunHealth;
                Evilhealth = Health;
                if(b == null)
                {

                }
                else
                {
                    if (b.bloonModel.baseId.Contains("Sun"))
                    {
                        HandleUI(b, true);
                    }
                    else if (b.bloonModel.baseId.Contains("Evil"))
                    {
                        HandleUI(b, false);
                    }
                }

                if (AscendedBosses.side == "evil")
                {
                    einfHp = true;
                    evilFHealth = 1;
                }
                else
                {
                    sinfHp = true;
                    sunFhealth = 1;
                }
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

        public static void HandleUI(Bloon b, bool sun)
        {
            if(instance != null && b != null)
            {
                if (sun)
                {
                    if(sinfHp)
                    {
                        Sunhealth.Text.text = sunFhealth + "/Inf";
                    }
                    else
                    {
                        Sunhealth.Text.text = sunFhealth + "/10000000";
                    }
                }
                else
                {
                    if (einfHp)
                    {
                        Evilhealth.Text.text = evilFHealth + "/Inf";
                    }
                    else
                    {
                        Evilhealth.Text.text = evilFHealth + "/10000000";
                    }
                }
                //for (int i = skulls.Count - 1; i >= 0; i--)
                //{
                //    if (skulls[i].RectTransform.anchoredPosition.x >= newWidth - (originalWidth / 2))
                //    {
                //        MelonCoroutines.Start(RemoveSkullWithEffect(skulls[i]));
                //    }
                //}
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

        [RegisterTypeInIl2Cpp]

        public class EvilBehavior : MonoBehaviour
        {
            public Bloon boss;
            public bool collide = false;
            public bool EndTrack = false;
            public bool ReturnToStart = false;
            public int maxHealth = 10000000;
            public EvilBehavior() : base()
            {
            }

            public void Start()
            {
                eboss = boss;
                InGame.instance.SpawnBloons(ModContent.BloonID<AscendedBosses.Sun>(), 1, 0);
                if (einfHp)
                {
                    evilFHealth = 1;
                }
            }

            public void Update()
            {
                if (boss != null)
                {
                    if (ReturnEvil)
                    {
                        collide = false;
                        ReturnToStart = false;
                        if (eboss.PercThroughMap() > 0.99f)
                        {
                            collide = true;
                            eboss.trackSpeedMultiplier = -0.5f;
                            ReturnToStart = true;
                            ReturnEvil = false;
                        }
                        else if (ReturnToStart == false)
                        {
                            collide = false;
                            eboss.trackSpeedMultiplier = 200;
                        }
                    }
                    if (einfHp && evilFHealth >= 10000000)
                    {
                        if (sboss != null && AscendedBosses.side == "evil")
                        {
                            InGame.Bridge.simulation.gameWon = true;
                            eboss.Destroy();
                            sboss.Destroy();
                            sunFhealth = 0f;
                        }
                    }
                    if (boss.PercThroughMap() > 0.99f)
                    {
                        collide = true;
                        boss.trackSpeedMultiplier = -0.5f;
                        EndTrack = true;
                    }
                    else if (EndTrack == false)
                    {
                        boss.trackSpeedMultiplier = 200;
                    }
                    FianlBossPos = new UnityEngine.Vector2(boss.Position.X, boss.Position.Z);
                    if (UnityEngine.Vector2.Distance(FianlBossPos,  FianlBoss1Pos) <= 2 && collide == true)
                    {
                        if (sboss != null && AscendedBosses.side == "good")
                        {
                            sboss.Destroy();
                            sunFhealth = 0f;
                        }
                        else if(eboss != null && AscendedBosses.side == "evil")
                        {
                            eboss.Destroy();
                            evilFHealth = 0f;
                        }
                    }

                    if(einfHp)
                    {
                        if (boss.health < boss.bloonModel.maxHealth)
                        {
                            evilFHealth += (boss.bloonModel.maxHealth - boss.health);
                        }

                        boss.health = boss.bloonModel.maxHealth;
                    }
                    else
                    {
                        if (boss.health < boss.bloonModel.maxHealth)
                        {
                            evilFHealth -= (boss.bloonModel.maxHealth - boss.health);
                        }

                        boss.health = boss.bloonModel.maxHealth;
                    }

                    evilFHealth = Math.Max(0, evilFHealth);

                    if (evilFHealth <= 0)
                    {
                        boss.Destroy();
                    }
                }
                else
                {
                    this.Destroy();
                }
            }
        }

        [RegisterTypeInIl2Cpp]
        public class SunBehavior : MonoBehaviour
        {
            public Bloon boss;
            public int maxHealth = 10000000;
            public bool ReturnToStart;
            public SunBehavior() : base()
            {
            }

            public void Start()
            {
                sboss = boss;
                boss.trackSpeedMultiplier = 0.5f;
                if(sinfHp)
                {
                    sunFhealth = 1;
                }
            }

            public void Update()
            {
                if (boss != null)
                {
                    if (Return)
                    {
                        sboss.Destroy();
                        Return = false;
                        InGame.instance.SpawnBloons(ModContent.BloonID<AscendedBosses.Sun>(), 1, 0);
                        FinalBossUI.CreatePanel(null);
                    }
                    if (sinfHp && sunFhealth >= 10000000)
                    {
                        if (eboss != null && AscendedBosses.side == "good")
                        {
                            InGame.Bridge.simulation.gameWon = true;
                            eboss.Destroy();
                            sboss.Destroy();
                            evilFHealth = 0f;
                        }
                    }
                    FianlBoss1Pos = new UnityEngine.Vector2(boss.Position.X, boss.Position.Z);

                    if(sinfHp)
                    {
                        if (boss.health < boss.bloonModel.maxHealth)
                        {
                            sunFhealth += (boss.bloonModel.maxHealth - boss.health);
                        }

                        boss.health = boss.bloonModel.maxHealth;
                    }
                    else
                    {
                        if (boss.health < boss.bloonModel.maxHealth)
                        {
                            sunFhealth -= (boss.bloonModel.maxHealth - boss.health);
                        }

                        boss.health = boss.bloonModel.maxHealth;
                    }

                    sunFhealth = Math.Max(0, sunFhealth);

                    if (sunFhealth <= 0)
                    {
                        boss.Destroy();
                    }
                }
                else
                {
                    this.Destroy();
                }
            }
        }
    }
}

