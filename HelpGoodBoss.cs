using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AscendedBosses;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Data.Behaviors.Towers;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.Achievements;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppAssets.Scripts.Utils;
using Il2CppTMPro;
using MelonLoader;
using StoryTemplate;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static AscendedBosses.AscendedBosses;
using static MelonLoader.MelonLogger;

namespace AscendedBosses
{
    [RegisterTypeInIl2Cpp(false)]
    public class HelpGoodBossUI : MonoBehaviour
    {
        public static HelpGoodBossUI? instance;

        public static int orbs;
        public void Close()
        {
            if (base.gameObject)
            {
                base.gameObject.Destroy();
            }
        }
        public static void CreatePanel()
        {
            RectTransform rect = InGame.instance.uiRect;
            ModHelperPanel p = null;
            if(AscendedBosses.side == "good")
            {
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0f, -1050f, 1250f, 600f), ModContent.GetTextureGUID<AscendedBosses>("GoodSidePanel"));
                instance = panel.AddComponent<HelpGoodBossUI>();
                ModHelperImage ShopIcon = panel.AddImage(new Info("Image_", -550, 350, 100, 100), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperImage GoodBuff1Icon = panel.AddImage(new Info("Image_", -325, 250, 50, 50), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperImage GoodBuff2Icon = panel.AddImage(new Info("Image_", 75, 250, 50, 50), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperImage GoodBuff3Icon = panel.AddImage(new Info("Image_", 475, 250, 50, 50), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperText ShopText = panel.AddText(new Info("ShopText_", -275, 350, 500, 250), "Buffs Shop", 70);
                ModHelperText GoodLine1 = panel.AddText(new Info("LineGood_", -400, 250, 500, 250), "100", 50);
                ModHelperText GoodLine2 = panel.AddText(new Info("LineGood_", 0, 250, 500, 250), "1000", 50);
                ModHelperText GoodLine3 = panel.AddText(new Info("LineGood_", 400, 250, 500, 250), "500", 50);
                GoodLine1.Text.color = Color.yellow;
                GoodLine2.Text.color = Color.yellow;
                GoodLine3.Text.color = Color.yellow;
                ShopText.Text.color = Color.yellow;
                ModHelperImage GoodBuff1 = panel.AddImage(new Info("Image_", -400, -50, 200, 500), ModContent.GetTextureGUID<AscendedBosses>("GoodTopPanel"));
                ModHelperImage GoodBuff2 = panel.AddImage(new Info("Image_", 0, -50, 200, 500), ModContent.GetTextureGUID<AscendedBosses>("GoodTopPanel"));
                ModHelperImage GoodBuff3 = panel.AddImage(new Info("Image_", 400, -50, 200, 500), ModContent.GetTextureGUID<AscendedBosses>("GoodTopPanel"));
                p = panel;
            }
            else
            {
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0f, -1050f, 1250f, 600f), ModContent.GetTextureGUID<AscendedBosses>("EvilSidePanel"));
                instance = panel.AddComponent<HelpGoodBossUI>();
                ModHelperImage ShopIcon = panel.AddImage(new Info("Image_", -550, 350, 100, 100), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperImage EvilBuff1Icon = panel.AddImage(new Info("Image_", -325, 250, 50, 50), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperImage EvilBuff2Icon = panel.AddImage(new Info("Image_", 75, 250, 50, 50), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperImage EvilBuff3Icon = panel.AddImage(new Info("Image_", 475, 250, 50, 50), ModContent.GetTextureGUID<AscendedBosses>("Orb"));
                ModHelperText ShopText = panel.AddText(new Info("ShopText_", -275, 350, 500, 250), "Buffs Shop", 70);
                ModHelperText EvilLine1 = panel.AddText(new Info("LineEvil_", -400, 250, 500, 250), "100", 50);
                ModHelperText EvilLine2 = panel.AddText(new Info("LineEvil_", 0, 250, 500, 250), "250", 50);
                ModHelperText EvilLine3 = panel.AddText(new Info("LineEvil_", 400, 250, 500, 250), "500", 50);
                EvilLine1.Text.color = Color.red;
                EvilLine2.Text.color = Color.red;
                EvilLine3.Text.color = Color.red;
                ShopText.Text.color = Color.red;
                ModHelperImage EvilBuff1 = panel.AddImage(new Info("Image_", -400, -50, 200, 500), ModContent.GetTextureGUID<AscendedBosses>("EvilTopPanel"));
                ModHelperImage EvilBuff2 = panel.AddImage(new Info("Image_", 0, -50, 200, 500), ModContent.GetTextureGUID<AscendedBosses>("EvilTopPanel"));
                ModHelperImage EvilBuff3 = panel.AddImage(new Info("Image_", 400, -50, 200, 500), ModContent.GetTextureGUID<AscendedBosses>("EvilTopPanel"));
                p = panel;
            }

            ModHelperButton Btn1 = p.AddButton(new Info("BossBtn1", -400, -50, 200, 400), ModContent.GetTextureGUID<AscendedBosses>("blank"), new Action(() => Buy(100)));
            ModHelperImage Btn1Img = Btn1.AddImage(new Info("BossBtnImg", 0, 0, 200, 400), VanillaSprites.JungleDrumsUpgradeIcon);

            ModHelperButton Btn2 = p.AddButton(new Info("BossBtn1", 0, -50, 200, 400), ModContent.GetTextureGUID<AscendedBosses>("blank"), new Action(() => Buy(250)));
            ModHelperImage Btn2Img = Btn2.AddImage(new Info("BossBtnImg", 0, 0, 200, 400), ModContent.GetTextureGUID<AscendedBosses>("Seperation"));

            ModHelperButton Btn3 = p.AddButton(new Info("BossBtn1", 400, -50, 200, 400), ModContent.GetTextureGUID<AscendedBosses>("blank"), new Action(() => Buy(500)));
            ModHelperImage Btn3Img = Btn3.AddImage(new Info("BossBtnImg", 0, 0, 200, 400), ModContent.GetTextureGUID<AscendedBosses>("godvil"));
        }

        public static void Buy(int o)
        {
            PopupScreen.instance.SafelyQueue(p =>
            {
                p.ShowPopup(
                    PopupScreen.Placement.menuCenter,
                    "Buy Buff",
                    $"Would you like to buy this buff for {o} buffs orbs? you have {orbs}!",
                    new Action(() =>
                    {
                        if(o == 100)
                        {
                            if(orbs >= o)
                            {
                                orbs -= o;
                                buffTowers();
                                p.ShowOkPopup("All towers gained 1 damage and 2 range!");
                            }
                            else
                            {
                                p.ShowOkPopup("You don't have enough orbs");
                            }
                        }
                        else if(o == 250)
                        {
                            if (orbs >= o)
                            {
                                orbs -= o;
                                FinalBossUI.Return = true;
                                FinalBossUI.ReturnEvil = true;
                                p.ShowOkPopup("Both bosses are returning to start");
                            }
                            else
                            {
                                p.ShowOkPopup("You don't have enough orbs");
                            }
                        }
                        else if (o == 500)
                        {
                            if (orbs >= o)
                            {
                                orbs -= o;
                                if(AscendedBosses.side == "evil")
                                {
                                    AscendedBosses.evilBuff = true;
                                    p.ShowOkPopup("bloon are twices as slow.");
                                }
                                else
                                {
                                    bool towerPlaced = false;
                                    Il2CppSystem.Action<bool> something = (Il2CppSystem.Action<bool>)delegate (bool s)
                                    {
                                        towerPlaced = s;
                                    };
                                    Il2CppSystem.Action<bool> spawn = something;

                                    InGame.instance.bridge.CreateTowerAt(new Vector2(RandomInt(-100, 100), RandomInt(-100, 100)), Game.instance.model.GetTowerFromId("SuperMonkey-400"), ObjectId.Create(9999, 0), false, something, true, true, false, 0);
                                    InGame.instance.bridge.CreateTowerAt(new Vector2(RandomInt(-100, 100), RandomInt(-100, 100)), Game.instance.model.GetTowerFromId("SuperMonkey-400"), ObjectId.Create(99991, 0), false, something, true, true, false, 0);
                                    InGame.instance.bridge.CreateTowerAt(new Vector2(RandomInt(-100, 100), RandomInt(-100, 100)), Game.instance.model.GetTowerFromId("SuperMonkey-400"), ObjectId.Create(99992, 0), false, something, true, true, false, 0);
                                    p.ShowOkPopup("May the sun be with you.");
                                }
                            }
                            else
                            {
                                p.ShowOkPopup("You don't have enough orbs");
                            }
                        }
                    }), "Buy", null, "Cancel", Popup.TransitionAnim.Scale, instantClose: true
                );
            });
        }

        public static int RandomInt(int min, int max)
        {
            System.Random rand = new();
            return rand.Next(min, max);
        }
        public static void buffTowers()
        {
            foreach (var t in InGame.instance.GetAllTowerToSim())
            {
                string[] BlackListedTowers =
                {
                        "Glue", "BananaFarm", "Village", "BeastHandler", "IceMonkey"
                };

                if (BlackListedTowers.Any(tb => t.tower.towerModel.baseId.ContainsIgnoreCase(tb)))
                {
                    continue;
                }

                var tm = t.tower.rootModel.Duplicate().Cast<TowerModel>();
                tm.range += 2;

                foreach (var a in tm.GetAttackModels())
                {
                    a.range += 5;
                    foreach (var w in a.weapons)
                    {
                        if (w.projectile.GetDamageModel() != null)
                        {
                            w.projectile.GetDamageModel().damage += 1;
                        }
                    }
                }

                t.tower.UpdateRootModel(tm);
            }
        }
    }
}
