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
using Il2CppAssets.Scripts.Data.Behaviors.Towers;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Utils;
using MelonLoader;
using StoryTemplate;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static MelonLoader.MelonLogger;

namespace StoryTemplate
{
    public class Story
    {
        public static Dictionary<int, Story.StoryMessage[]> Stories = new Dictionary<int, Story.StoryMessage[]>();
        public struct StoryMessage
        {
            public StoryMessage(string message, string name, Story.StoryPortrait portrait, bool PausesGame)
            {
                this.Message = message;
                this.Name = name;
                this.Portrait = portrait;
                this.pausesGame = PausesGame;
            }

            public string Message;
            public string Name;
            public Story.StoryPortrait Portrait;
            public bool pausesGame;
        }

        public enum StoryPortrait
        {
            Player,
            mysterious,
            AsmodeusLvL1,
            Helios,
            PlayerNoWay
        }


        [RegisterTypeInIl2Cpp(false)]
        public class StoryUI : MonoBehaviour
        {
            public static StoryUI? instance;
            public static ModHelperText MessageText = null;
            public static ModHelperImage PortraitImage = null;
            public static ModHelperText NameText = null;
            public static ModHelperButton closeBtn = null;
            public static ModHelperButton continueBtn = null;
            public static StoryMessage[] Msgs = null;
            public static ModHelperPanel storyPanel = null;
            public static int CurrMsg = 0;
            public void Close()
            {
                if (base.gameObject)
                {
                    base.gameObject.Destroy();
                }
            }
            public static void CreatePanel(StoryMessage[] msgs)
            {
                Reset();
                Msgs = msgs;
                var Length = msgs.Length;
                RectTransform rect = InGame.instance.uiRect;
                ModHelperPanel panel = rect.gameObject.AddModHelperPanel(new("Panel_", 0f, -1000f, 1250f, 600f), VanillaSprites.BrownPanel);
                instance = panel.AddComponent<StoryUI>();
                storyPanel = panel;

                TimeManager.gamePaused = msgs[CurrMsg].pausesGame;

                ModHelperText nameText = panel.AddText(new Info("Name_", -400f, 300f, 500f, 250f), "");
                nameText.transform.localScale *= 2f;
                if(Length > 1)
                {
                    ModHelperButton ContinueBtn = panel.AddButton(new Info("ContinueBtn", 625f, 300f, 100f), VanillaSprites.ContinueBtn, new Action(() => Continue(Length)));
                    continueBtn = ContinueBtn;
                }
                else
                {
                    ModHelperButton CloseBtn = panel.AddButton(new Info("CloseBtn", 625f, 300f, 100f), VanillaSprites.CloseBtn, new Action(() => instance.Close()));
                    closeBtn  = CloseBtn;
                }

                ModHelperImage portrait = panel.AddImage(new Info("StoryImage_", 1000f, 0f, 750f, 750f), ModContent.GetTextureGUID<AscendedBosses.AscendedBosses>(msgs[CurrMsg].Portrait.ToString()));
                ModHelperText text_ = panel.AddText(new Info("Title_", 0f, 0f, 1150f, 500f), msgs[CurrMsg] .Message ?? "");

                PortraitImage = portrait;
                MessageText = text_;
                NameText = nameText;

                text_.Text.enableAutoSizing = true;
                string nameToUse = (msgs[CurrMsg].Name == "Player") ? (Game.LiNKDisplayName ?? "") : (msgs[CurrMsg].Name ?? "");
                nameText.Text.text = nameToUse;
                if (nameToUse == "???")
                {
                    ChangeNameColor(UnityEngine.Color.red);
                }
                else if (nameToUse == "Asmodeus") 
                {
                    ChangeNameColor(UnityEngine.Color.red);
                }
                else if (nameToUse == "Helios") 
                {
                    ChangeNameColor(UnityEngine.Color.yellow);
                }
                else { }
            }
        }

        public static void Continue(int totalMsgs)
        {
            if (StoryUI.Msgs == null || StoryUI.Msgs.Length == 0) return;

            StoryUI.CurrMsg++;

            if (StoryUI.CurrMsg < totalMsgs)
            {
                StoryMessage currentMsg = StoryUI.Msgs[StoryUI.CurrMsg];
                TimeManager.gamePaused = currentMsg.pausesGame;
                StoryUI.MessageText.Text.text = currentMsg.Message;
                StoryUI.NameText.Text.text = currentMsg.Name;
                if (StoryUI.NameText.Text.text == Game.LiNKDisplayName)
                {
                    ChangeNameColor(UnityEngine.Color.white);
                }
                else if(StoryUI.NameText.Text.text == "???")
                {
                    ChangeNameColor(UnityEngine.Color.red);
                }
                else if (StoryUI.NameText.Text.text == "Asmodeus")
                {
                    ChangeNameColor(UnityEngine.Color.red);
                }
                else if (StoryUI.NameText.Text.text == "Helios")
                {
                    ChangeNameColor(UnityEngine.Color.yellow);
                }
                if (StoryUI.PortraitImage != null)
                {
                    StoryUI.PortraitImage.Destroy();
                }

                GameObject existingStoryImage = GameObject.Find("StoryImage_");
                if (existingStoryImage)
                {
                    existingStoryImage.Destroy();
                }

                StoryUI.PortraitImage = StoryUI.storyPanel.AddImage(
                    new Info("StoryImage_", 1000f, 0f, 750f, 750f),
                    ModContent.GetTextureGUID<AscendedBosses.AscendedBosses>(currentMsg.Portrait.ToString())
                );

                if (StoryUI.CurrMsg == totalMsgs - 1)
                {
                    if (StoryUI.continueBtn != null)
                    {
                        StoryUI.continueBtn.Destroy();
                        StoryUI.continueBtn = null;
                    }

                    if (StoryUI.closeBtn == null)
                    {
                        StoryUI.closeBtn = StoryUI.storyPanel.AddButton(
                            new Info("CloseBtn", 625f, 300f, 100f),
                            VanillaSprites.CloseBtn,
                            new Action(() => StoryUI.instance.Close())
                        );
                    }
                }
            }
        }

        public static void Reset()
        {
            StoryUI.MessageText = null;
            StoryUI.PortraitImage = null;
            StoryUI.NameText = null;
            StoryUI.closeBtn = null;
            StoryUI.continueBtn = null;
            StoryUI.Msgs = null;
            StoryUI.storyPanel = null;
            StoryUI.CurrMsg = 0;
        }
        public static void ChangeNameColor(Color color)
        {
            StoryUI.NameText.Text.color = color;
        }
    }
}
