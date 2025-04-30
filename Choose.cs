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
    public class ChooseUI : MonoBehaviour
    {
        public static ChooseUI instance;
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
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new("Panel_", -750, 100, 650, 1750), ModContent.GetTextureGUID<AscendedBosses>("EvilBack"));
                instance = panel.AddComponent<ChooseUI>();
                ModHelperImage panelInside = panel.AddImage(new Info("Image_", 0, 0, 750, 2000), ModContent.GetTextureGUID<AscendedBosses>("EvilFront"));
                ModHelperImage secondPanel = panel.AddImage(new Info("Image_", 850, 0, 750, 2000), ModContent.GetTextureGUID<AscendedBosses>("goodback"));
                ModHelperImage secondPanelInside = secondPanel.AddImage(new Info("Image_", 0, 100, 650, 1750), ModContent.GetTextureGUID<AscendedBosses>("goodfront"));

                ModHelperText EvilText = panel.AddText(new Info("Evil_", 0, 625, 500, 250), "EVIL", 100);
                EvilText.Text.color = Color.red;
                ModHelperText GoodText = secondPanel.AddText(new Info("Good_", 0, 625, 500, 250), "GOOD", 100);
                GoodText.Text.color = Color.yellow;

                ModHelperButton EvilBtn = panel.AddButton(new Info("EvilButton_", 0, -1250 / 2, 600, 200), VanillaSprites.RedBtnLong, new Action(() => Choose(true)));
                ModHelperButton GoodBtn = secondPanel.AddButton(new Info("EvilButton_", 0, -1250 / 2, 600, 200), VanillaSprites.YellowBtnLong, new Action(() => Choose(false)));
                EvilBtn.AddText(new Info("Title_", 0f, 0f, 300, 150), "Choose", 70);
                GoodBtn.AddText(new Info("Title_", 0f, 0f, 300, 150), "Choose", 70);

                ModHelperText EvilLine1 = panel.AddText(new Info("LineEvil_", 0, 500, 500, 250), "Towers are cheaper", 50);
                ModHelperText EvilLine2 = panel.AddText(new Info("LineEvil_", 0, 350, 500, 250), "1000 End of round cash", 50);
                ModHelperText EvilLine3 = panel.AddText(new Info("LineEvil_", 0, 200, 500, 250), "Cursed bloon won't spawn", 50);
                ModHelperText EvilLine4 = panel.AddText(new Info("LineEvil_", 0, 50, 500, 250), "Tower randomly get upgraded to tier 5", 50);
                ModHelperText EvilLine5 = panel.AddText(new Info("LineEvil_", 0, -100, 500, 250), "All bloons have half health", 50);
                ModHelperText EvilLine6 = panel.AddText(new Info("LineEvil_", 0, -250, 500, 250), "Farm make two times more cash", 50);
                ModHelperText EvilLine7 = panel.AddText(new Info("LineEvil_", 0, -400, 500, 250), "Free continues", 50);

                ModHelperText GoodLine1 = secondPanel.AddText(new Info("LineGood_", 0, 500, 500, 250), "Towers are 10% more expensive", 50);
                ModHelperText GoodLine2 = secondPanel.AddText(new Info("LineGood_", 0, 350, 500, 250), "All bloons have x2 health", 50);
                ModHelperText GoodLine3 = secondPanel.AddText(new Info("LineGood_", 0, 200, 500, 250), "Tower randomly get sold", 50);
                ModHelperText GoodLine4 = secondPanel.AddText(new Info("LineGood_", 0, 50, 500, 250), "All moabs turn into bfb", 50);
                ModHelperText GoodLine5 = secondPanel.AddText(new Info("LineGood_", 0, -100, 500, 250), "Cash Randomly Get Stolen By Bloons", 50);
                ModHelperText GoodLine6 = secondPanel.AddText(new Info("LineGood_", 0, -250, 500, 250), "Banana farm make no money", 50);
                ModHelperText GoodLine7 = secondPanel.AddText(new Info("LineGood_", 0, -400, 500, 250), "1 life no continue", 50);
            }
        }

        public static void Choose(bool b)
        {
            instance.Close();
            if(b)
            {
                AscendedBosses.side = "evil";
                MelonLogger.Msg("Player choosed evil");
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("YOU FOOL!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("You fell right into my trap.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("You should be at my mercy.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("HOW DARE YOU RIG THE VOTE?!", Game.LiNKDisplayName, Story.StoryPortrait.PlayerNoWay, true),
                    new Story.StoryMessage("I'm the devil, I do what I want.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Can I at least have my end of round cash?", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
                    new Story.StoryMessage("Urgh, fine.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false)
                };
                EvilBuffs.CreatePanel();
                InGame.instance.SetHealth(1);
                Story.StoryUI.CreatePanel(messages);
                debuffTowers();
                towerstolock();
            }
            else
            {
                AscendedBosses.side = "good";
                MelonLogger.Msg("Player choosed good");
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Welcome to the good side.", "Helios", Story.StoryPortrait.Helios, true),
                    new Story.StoryMessage("Good job not getting tricked by my brother, Asmodeus.", "Helios", Story.StoryPortrait.Helios, true),
                    new Story.StoryMessage("So all these other buffs were fake?", Game.LiNKDisplayName, Story.StoryPortrait.PlayerNoWay, true),
                    new Story.StoryMessage("Yes, exactly. He tried rigging the vote.", "Helios", Story.StoryPortrait.Helios, true),
                    new Story.StoryMessage("Here, my actual buff.", "Helios", Story.StoryPortrait.Helios, false),
                };
                GoodBuffs.CreatePanel();
                InGame.instance.SetHealth(150);
                Story.StoryUI.CreatePanel(messages);
                buffTowers();
            }
        }
        public static void debuffTowers()
        {
            foreach (var t in InGame.instance.GetAllTowerToSim())
            { 
                var tm = t.tower.rootModel.Duplicate().Cast<TowerModel>();
                tm.range -= 5;

                foreach (var a in tm.GetAttackModels())
                {
                    a.range -= 5;
                }
                t.tower.UpdateRootModel(tm);
            }
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
                tm.range += 5;

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

        public static void towerstolock()
        {
            List<string> towerIds = new List<string>();

            foreach (var t in Game.instance.model.GetAllTowerDetails())
            {
                towerIds.Add(t.towerId);
            }

            System.Random rand = new System.Random();
            var selectedTowers = towerIds.OrderBy(x => rand.Next()).Take(2).ToList();

            MelonLogger.Msg($"Selected Towers: {selectedTowers[0]}, {selectedTowers[1]}");
            AscendedBosses.FirstTowerToLock = selectedTowers[0];
            AscendedBosses.SecondTowerToLock = selectedTowers[1];

            PopupScreen.instance.ShowOkPopup($"Both the {selectedTowers[0]} and {selectedTowers[1]} were locked meaning you can't use them");
        }
    }
}

