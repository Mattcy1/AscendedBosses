using BTD_Mod_Helper;
using MelonLoader;
using AscendedBosses;
using Il2CppAssets.Scripts.Models;
using StoryTemplate;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Objects;
using System;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Unity.UI_New.Achievements;
using BTD_Mod_Helper.Api.Bloons;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Bloons;
using Il2CppAssets.Scripts.Unity.Display;
using System.Collections.Generic;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Simulation.Bloons;
using HarmonyLib;
using BTD_Mod_Helper.Api;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using BTD_Mod_Helper.Api.ModOptions;
using UnityEngine;
using Il2CppAssets.Scripts.Simulation.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors.Actions;
using CommandLine;
using static MelonLoader.MelonLogger;
using System.IO;
using UnityEngine.UI;
using MelonLoader.Utils;
using Il2CppAssets.Scripts.Data.GameEditor;
using BTD_Mod_Helper.Api.Hooks.BloonHooks;
using BTD_Mod_Helper.Api.Hooks;
using Il2Cpp;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;

[assembly: MelonInfo(typeof(AscendedBosses.AscendedBosses), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace AscendedBosses;

public class AscendedBosses : BloonsTD6Mod
{
    public static string side = "none";
    public static double cashtoadd = 100;
    public static int roundToGiveCash = 10;
    public static string FirstTowerToLock = "";
    public static string SecondTowerToLock = "";
    public static bool evilBuff = false;

    public static string[] Bosses = new string[]
    {
        new string("Boss1"),
        new string("Boss2"),
        new string("Boss3")
    };
    public override void OnApplicationStart()
    {
        ModHelper.Msg<AscendedBosses>("AscendedBosses loaded!");
    }

    public override void OnMainMenu()
    {
    }

    public override void OnNewGameModel(GameModel result)
    {
        Story.StoryMessage[] messages = new Story.StoryMessage[]
        {
            new Story.StoryMessage("Greetings...", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("I've heard many rumors about you...", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("Good ones at that.", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("What are they saying about me?", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
            new Story.StoryMessage("Well, the fact that you’re a god and control the entire Monkey army.", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("How do you know this?", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
            new Story.StoryMessage("Don’t ask. What I’m here to inform you about is a great deal!", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("Are you like the devil and I’m gonna sell you my soul or something? What’s going on here?", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
            new Story.StoryMessage("There’s been a massive outbreak in the bloon world, left and right, bloons everywhere!", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("We need you to decide whether you want to help us dev- I mean, uhhh… stronger guys… take on the new onslaught of Ascended bloons!", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("Or be a poopyhead and stay on the good side.", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("What’s in it for me if I join you?", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
            new Story.StoryMessage("Well, I’m glad you asked! I’ll make the prices of towers much cheaper for you and I’ll give you lots more money! Like, so much money you’ll be swimming in a pool of coins like Scrooge McDuck!", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("What if I stay where I am? On the good side?", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
            new Story.StoryMessage("Well that’s a horrible idea if I were you. I’d be commanding the entire Bloon army at you!", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("But won’t the Ascended bloons be stronger than your old bloons?", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
            new Story.StoryMessage("And also, joining you means I lose the power of the sun god.", Game.LiNKDisplayName, Story.StoryPortrait.Player, true),
            new Story.StoryMessage("Uhhhh n-n-no you don’t!! You get bad stuff when you stay with the sun god! Trust me!", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("A-a-and… Ummm…", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("If you join me, I’ll give you a free cookie!", "???", Story.StoryPortrait.mysterious, true),
            new Story.StoryMessage("I don’t know about this…", Game.LiNKDisplayName, Story.StoryPortrait.Player, false)
        };

        Story.StoryUI.CreatePanel(messages);
    }

    public override void OnRoundStart()
    {
        var currRound = InGame.instance.bridge.GetCurrentRound() + 1;
        if (currRound == 4)
        {
            ChooseUI.CreatePanel();
        }

        if(side == "good")
        {
            if (currRound == 10)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("I despise Asmodeus!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("His stupid evil bloons are ruining everything!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Always scheming, always spawning more trouble!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("One day, I'm gonna pop every last one of his bloons myself!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("But for now, I can just have you do it.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("It doesn't seem too far-fetched for you to deal with at the moment.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Once it gets really challenging, I'll come in and help myself.", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 19)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("I heard... you get to pick which boss comes next?", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("That's terrifying!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("You're smart though... you’ll pick the right one, right?", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("I’m just gonna, uh, hide over here for a bit... good luck!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 25)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Hey, you're still alive!", "Helios", Story.StoryPortrait.Helios, false),
                new Story.StoryMessage("How did it go?!", "Helios", Story.StoryPortrait.Helios, false),
                new Story.StoryMessage("You crushed that boss, didn’t you?", "Helios", Story.StoryPortrait.Helios, false),
                new Story.StoryMessage("I knew you could do it!", "Helios", Story.StoryPortrait.Helios, false),
                new Story.StoryMessage("Alright, let's keep the momentum rolling!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
                BossChoose.CreatePanel(40);
            }
            else if (currRound == 30)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Okay, real talk.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("You should start farming some bananas.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Big bosses are coming... and money solves problems!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Delicious, golden bananas... mmm.", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 39)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Boss on the horizon... round 40!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("I’m scared, but I believe in you!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("We're stronger than those evil bloons!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Get ready to give it everything you've got!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 40)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("This is it!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Charge, monkey army!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Smash that big bloon into mush!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 50)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Yawn...", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Is it just me or is it getting kinda boring?", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Maybe I should sing a song? Or do a little dance?", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Or maybe we can read a book together!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("...no? Okay. I'll just... stay quiet now.", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 59)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Hey, heads up!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Big boss incoming at round 60!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Pick your boss wisely, dude!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("I believe in your choice!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 65)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Whoa!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("You're handling these bosses way better than I expected!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("You're like... a boss-bloon-busting machine!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 70)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Sigh...", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("This is getting really boring now.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("I’m just gonna... take a little nap.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Wake me up if something cool happens, okay?", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 80)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("AAH! WHAT’S HAPPENING?!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Oh, right, the boss thingy.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("I wasn't sleeping, I was... uhh... meditating!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("But you can still do this!!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 85)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Uhh... I just got a call...", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("It was Asmodeus...", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("He said we're doomed at round 100!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("He said something about a massive ahh blimp but...", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("What do we do?!", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 90)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Okay, okay, let's think! We gotta prepare for this crap.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("What if... Asmodeus summons a bloon the size of the SUN?!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Or maybe... a bloon made out of lava?!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Or... a giant evil banana blimp???", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("No, no, no, that doesn't make sense...", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("I think you just gotta farm a crap ton.", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 95)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Okay dude you should probably stop farming now", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Sell all those bananer' farms and get some paragoons!!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Maybe even a VTSG....", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Nah, I would probably just stick to getting some high degree paragons if I were you. Maybe a VTSG if you got leftover money.", "Helios", Story.StoryPortrait.Helios, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 100)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                new Story.StoryMessage("Uhh... that thing looks kinda rickety and...", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Honestly dumb.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Its a good thing I got THIS bad boy! I call it the Helioso-matic!!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("Sure it's a crappy ahh name but it's better than nothing, and we don't have time to decide a better one.", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("So pop Asmodeus's blimp and use those bloony-orb thingies to use my friendly bloon's special abilities to help!", "Helios", Story.StoryPortrait.Helios, true),
                new Story.StoryMessage("One of them spawns a bunch of Sun Gods around the map! Isn't that cool?", "Helios", Story.StoryPortrait.Helios, false),
                };

                InGame.instance.SpawnBloons(ModContent.BloonID<Evil>(), 1, 0);
                Story.StoryUI.CreatePanel(messages);
            }
        }

        if(side == "evil")
        {
            if (currRound == 10)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Ugh, Helios again...", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("I can't believe that shiny idiot still thinks he can stop me!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("His bloons are so... sparkly and stupid!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Don't worry, servant, you'll crush his garbage for me.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("And if you don't, well, that's your problem!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 19)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Okay okay, listen.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Apparently, an Elite Boss bloon is coming next round.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("You get to choose which one it is... so don't pick something too scary, alright?!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("I'll, uh... just stand behind you. For... tactical reasons!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
            }
            else if (currRound == 20)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Alright you got th- WHAT THE HELL IS THAT", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("WHY DOES IT HAVE SO MUCH HP AND SO MANY SKULLS", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("HOLY CRAP WE'RE GONNA DIE!!! AHHHHHHH", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 25)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("You're... still alive?", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("About time you did something right!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Helios's little boss didn't stand a chance against my forces!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Maybe you're not completely useless after all!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 30)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("dude you need more money!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Start farming or something, but like, for real now", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("I won't lift a finger to help you if you go broke!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Not counting the $1000 extra dollars i'm legally required to give you every round...", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 39)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Ascended boss on the way at round 40!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Not that I'm scared or anything!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("I just, uh, value strategic retreats if things go wrong!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("So don't mess this up!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 40)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Attack! Attack!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Ugh... I'm not cut out for this sorta thing...", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Just do your thing okay?", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Don't die.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 50)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("huh? Oh, you're still here.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("This is getting so boring I might actually die again!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Maybe we should uhh...", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("play some minecraft together in the meantime?", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("maybe some TF2?", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("or Arras.io?", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Ugh, forget it.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 59)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Hey poopy head, another Ascended boss is coming at round 60!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Pick one that won't destroy us immediately, like last time okay??", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("And if it does I'm blaming you because everything is your fault", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 65)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Huh... you're actually winning?", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Color me impressed.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Maybe selling your soul wasn't a total waste!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Don't let it get to your head, though", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 70)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("......", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("This is so boring dude", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Maybe I'll take a nap or something... wake me when something explodes.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 80)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("AHHHHHHHHHHH!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("What's that?!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Oh wait another ascended boss incoming!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("I was NOT sleeping!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Defend me! I mean, defend the battlefield!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 85)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("huh? * He's on his cell phone *", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Helios?!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("He says you're doomed at round 100!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("I'm not scared! You're scared!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("* hangs up furiously * oh man, we're so doomed.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 90)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("Okay, think, think, THINK!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("What if Helios summons a bloon that's, like... made of pure hatred?!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Or even worse a blimp that's made of good stuff!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Or... or... a bloon that's just an infinite black hole?!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("Or a bloon that's just a million bloons stacked on each other?!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("We're so, so doomed.", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("And the worst part?", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("He's probably gonna roll up in the most amazing thing ever that you can't pop it by yourself...", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("And the only way I can help is by slapping together a couple wood planks and eternal flames to fight it!", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };
                Story.StoryUI.CreatePanel(messages);
            }
            else if (currRound == 100)
            {
                Story.StoryMessage[] messages = new Story.StoryMessage[]
                {
                    new Story.StoryMessage("welp, here goes nothing", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("heres my goofy ahh makeshift blimp", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, true),
                    new Story.StoryMessage("please dont let me down", "Asmodeus", Story.StoryPortrait.AsmodeusLvL1, false),
                };

                InGame.instance.SpawnBloons(ModContent.BloonID<Evil>(), 1, 0);
                Story.StoryUI.CreatePanel(messages);
            }
        }


        if (side == "good")
        {
            if (currRound == 40)
            {
                if (Bosses[0] == "Bloonarius")
                {
                    InGame.instance.SpawnBloons(ModContent.BloonID<AscendedBloonarius>(), 1, 0);
                }
                else
                {
                    InGame.instance.SpawnBloons(ModContent.BloonID<AscendedVortex>(), 1, 0);
                }
            }

            if (currRound == 60)
            {
                if (Bosses[1] == "Phayze")
                {
                    InGame.instance.SpawnBloons(ModContent.BloonID<AscendedPhayze>(), 1, 0);
                }
                else
                {
                    InGame.instance.SpawnBloons(ModContent.BloonID<AscendedLych>(), 1, 0);
                }
            }

            if (currRound == 80)
            {
                if (Bosses[2] == "Blast")
                {
                    InGame.instance.SpawnBloons(ModContent.BloonID<AscendedBlast>(), 1, 0);
                }
                else
                {
                    InGame.instance.SpawnBloons(ModContent.BloonID<AscendedDread>(), 1, 0);
                }
            }
        }
        else
        {
            if (currRound == 40)
            {
                InGame.instance.SpawnBloons(ModContent.BloonID<AscendedVortex>(), 1, 0);
            }

            if (currRound == 60)
            {
                InGame.instance.SpawnBloons(ModContent.BloonID<AscendedLych>(), 1, 0);
            }

            if (currRound == 80)
            {
                InGame.instance.SpawnBloons(ModContent.BloonID<AscendedDread>(), 1, 0);
            }
        }

        if (currRound == roundToGiveCash)
        {
            if (side == "evil")
            {
                InGame.instance.AddCash(1000);
                roundToGiveCash += 10;
            }
        }

        if (side == "good")
        {
            InGame.instance.AddCash(cashtoadd);
            cashtoadd += 25;
        }
    }

    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        System.Random rand = new System.Random();


        if (side == "evil")
        {
            if (tower.towerModel.baseId.Contains(FirstTowerToLock))
            {
                tower.SellTower();
            }
            else if (tower.towerModel.baseId.Contains(SecondTowerToLock))
            {
                tower.SellTower();
            }
        }
        if (rand.Next(20) == 0)
        {
            tower.worth = 0;
            tower.SellTower();
        }
        else
        {
            if (side == "good")
            {
                string[] BlackListedTowers =
                {
                    "Glue", "BananaFarm", "Village", "BeastHandler", "IceMonkey"
                };


                if (BlackListedTowers.Any(tb => tower.towerModel.baseId.ContainsIgnoreCase(tb)))
                {
                    return;
                }

                var tm = tower.rootModel.Duplicate().Cast<TowerModel>();
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

                tower.UpdateRootModel(tm);
            }
            else if (side == "evil")
            {
                var tm = tower.rootModel.Duplicate().Cast<TowerModel>();
                tm.range -= 5;

                foreach (var a in tm.GetAttackModels())
                {
                    a.range -= 5;
                }

                tower.UpdateRootModel(tm);
            }
        }
    }

    public override void OnTowerUpgraded(Tower tower, string upgradeName, TowerModel newBaseTowerModel)
    {
        if (side == "good")
        {
            string[] BlackListedTowers =
            {
                "Glue", "BananaFarm", "Village", "BeastHandler", "IceMonkey"
            };


            if (tower.towerModel.baseId.ContainsIgnoreCase("ninja") && tower.towerModel.tiers[1] == 5)
            {
                tower.SellTower();
            }
            if (BlackListedTowers.Any(tb => tower.towerModel.baseId.ContainsIgnoreCase(tb)))
            {
                return;
            }

            var tm = tower.rootModel.Duplicate().Cast<TowerModel>();
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

            tower.UpdateRootModel(tm);
        }
        else if (side == "evil")
        {
            var tm = tower.rootModel.Duplicate().Cast<TowerModel>();
            tm.range -= 5;

            foreach (var a in tm.GetAttackModels())
            {
                a.range -= 5;
            }

            tower.UpdateRootModel(tm);
        }
    }
    public override void OnRestart()
    {
        if (FinalBossUI.instance != null)
        {
            FinalBossUI.instance.Close();
        }
        //InGame.instance.SpawnBloons(ModContent.BloonID<Evil>(), 1, 400);
        //InGame.instance.SpawnBloons(ModContent.BloonID<AscendedBloonarius>(), 1, 400);
        //InGame.instance.SpawnBloons(ModContent.BloonID<AscendedBlast>(), 1, 400);
        //InGame.instance.SpawnBloons(ModContent.BloonID<AscendedDread>(), 1, 400);
        //InGame.instance.SpawnBloons(ModContent.BloonID<AscendedBloonarius>(), 1, 400);
        //InGame.instance.SpawnBloons(ModContent.BloonID<AscendedPhayze>(), 1, 400);
        //InGame.instance.SpawnBloons(ModContent.BloonID<AscendedVortex>(), 1, 400);
    }

    public static ModSettingHotkey GiveXP = new(KeyCode.B)
    {
        description = ""
    };

    public override void OnUpdate()
    {
        if (InGame.instance != null && InGame.instance.bridge != null)
        {
            foreach (var bloon in InGame.instance.GetBloons())
            {
                if (bloon != null && bloon.bloonModel.isMoab && !bloon.bloonModel.isBoss && bloon.HasTag("Cursed"))
                {
                    MelonCoroutines.Start(LoadMoab(bloon));
                    bloon.UpdateDisplay();
                }
            }
        }
    }

    public static System.Collections.IEnumerator LoadMoab(Bloon bloon)
    {
        yield return new WaitForSeconds((0.3f));
        if (bloon != null)
        {
            foreach (var renderer in bloon.GetUnityDisplayNode().GetMeshRenderers())
            {
                renderer.SetMainTexture(ModContent.GetTexture<AscendedBosses>(bloon.bloonModel.baseId + "Cursed"));
            }
        }
    }
    public class AscendedBloonarius : ModBloon
    {
        public override string BaseBloon => "Bloonarius1";
        public override string Icon => "AscendedBloonarius-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 75000;

            HealthPercentTriggerModel health = bloonModel.GetBehavior<HealthPercentTriggerModel>();
            health.percentageValues = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };

            SpawnBloonsActionModel spawn = bloonModel.GetBehavior<SpawnBloonsActionModel>();
            spawn.bloonType = "Moab";
            spawn.spawnCount = 1;
            spawn.spawnTrackMin = 0.3f;
            spawn.spawnTrackMax = 0.5f;

            TimeTriggerModel time = new TimeTriggerModel("SpawnBloonsTimed", 20, false, new string[] { "SpawnBloonsTimed" });

            SpawnBloonsActionModel spawn1 = Game.instance.model.GetBloon("Bloonarius1").GetBehavior<SpawnBloonsActionModel>().Duplicate();
            spawn1.bloonType = "Ceramic";
            spawn1.actionId = "SpawnBloonsTimed";
            spawn1.spawnCount = 5;
            spawn1.spawnTrackMin = 0.3f;
            spawn1.spawnTrackMax = 0.5f;
            spawn1.bossName = "Bloonarius";

            bloonModel.AddBehavior(spawn1);
            bloonModel.AddBehavior(time);
        }
    }
    public class AscendedVortex : ModBloon
    {
        public override string BaseBloon => "Vortex1";
        public override string Icon => "AscendedVortex-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 65000;
            bloonModel.speed *= 2f;

            HealthPercentTriggerModel health = bloonModel.GetBehavior<HealthPercentTriggerModel>();
            health.percentageValues = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };

            BuffBloonSpeedModel BuffBloonSpeedModel = bloonModel.GetBehavior<BuffBloonSpeedModel>();
            BuffBloonSpeedModel.speedBoost = 3f;
            StunTowersInRadiusActionModel stun = bloonModel.GetBehavior<StunTowersInRadiusActionModel>();
            stun.stunDuration = 20f;
            stun.radius = 80f;
        }
    }

    public class AscendedLych : ModBloon
    {
        public override string BaseBloon => "Lych1";
        public override string Icon => "AscendedLych-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 300000;
            bloonModel.speed *= 1.2f;

            HealthPercentTriggerModel health = bloonModel.GetBehavior<HealthPercentTriggerModel>();
            health.percentageValues = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };

            AbsorbTowerBuffsActionModel AbsorbTowerBuffsActionModel = bloonModel.GetBehavior<AbsorbTowerBuffsActionModel>();
            AbsorbTowerBuffsActionModel.healPercentPerBuff = 0.05f;
            HealOnTowerSellActionModel HealOnTowerSellAction = bloonModel.GetBehavior<HealOnTowerSellActionModel>();
            HealOnTowerSellAction.healPercentForHighestTier = 0.03f;
            ReanimateMoabsActionModel ReanimateMoabsActionModel = bloonModel.GetBehavior<ReanimateMoabsActionModel>();
            ReanimateMoabsActionModel.healthMultiplier = 7f;
            ReanimateMoabsActionModel.speedMultiplier = 2f;

            bloonModel.ApplyDisplay<LT>();
        }
    }

    public class AscendedPhayze : ModBloon
    {
        public override string BaseBloon => "Phayze1";
        public override string Icon => "AscendedPhayze-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 300000;
            bloonModel.speed *= 1.2f;
            //HealthPercentTriggerModel health = bloonModel.GetBehavior<HealthPercentTriggerModel>();
            //health.percentageValues = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };

            DashForwardsActionModel DashForwardsActionModel = bloonModel.GetBehavior<DashForwardsActionModel>();
            DashForwardsActionModel.dashDistance = 0.09f;

            GenerateShieldActionModel Shield = bloonModel.GetBehavior<GenerateShieldActionModel>();
            Shield.amount = 5000;

            PhayzeBehaviorModel PhayzeBehaviorModel = bloonModel.GetBehavior<PhayzeBehaviorModel>();
            PhayzeBehaviorModel.shieldSpeedBoost = 1.7f;
        }
    }

    public class BLT : ModBloonDisplay<AscendedBlast>
    {
        public override string BaseDisplay => Game.instance.model.GetBloon("BlastapopoulosElite5").GetDisplayGUID();
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.SetMainTexture(ModContent.GetTexture<AscendedBosses>("AscendedBlastT"));
            }
        }
    }

    public class PT : ModBloonDisplay<AscendedPhayze>
    {
        public override string BaseDisplay => Game.instance.model.GetBloon("PhayzeElite5").GetDisplayGUID();
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.SetMainTexture(ModContent.GetTexture<AscendedBosses>("AscendedPhayzeT"));
            }
        }
    }
    public class DT : ModBloonDisplay<AscendedDread>
    {
        public override string BaseDisplay => Game.instance.model.GetBloon("DreadbloonElite5").GetDisplayGUID();
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                if(!renderer.name.Contains("RockArmor"))
                {
                    renderer.SetMainTexture(ModContent.GetTexture<AscendedBosses>("AscendedDreadbloonT"));
                }
            }
            node.PrintInfo();
        }
    }
    public class BT : ModBloonDisplay<AscendedBloonarius>
    {
        public override string BaseDisplay => Game.instance.model.GetBloon("BloonariusElite5").GetDisplayGUID();
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.SetMainTexture(ModContent.GetTexture<AscendedBosses>("AscendedBloonariusT"));
            }
        }
    }

    public class VT : ModBloonDisplay<AscendedVortex>
    {
        public override string BaseDisplay => Game.instance.model.GetBloon("VortexElite5").GetDisplayGUID();
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.SetMainTexture(ModContent.GetTexture<AscendedBosses>("AscendedVortexT"));
            }
        }
    }

    public class LT : ModBloonDisplay<AscendedLych>
    {
        public override string BaseDisplay => Game.instance.model.GetBloon("LychElite5").GetDisplayGUID();
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.GetMeshRenderers())
            {
                renderer.SetMainTexture(ModContent.GetTexture<AscendedBosses>("AscendedLychT"));
            }
        }
    }

    public class EvilTex : ModBloonCustomDisplay<Evil>
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "AsmodeusBloon";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var n in node.GetRenderers())
            {
                n.ApplyOutlineShader();
            }
            SetMeshOutlineColor(node, UnityEngine.Color.red);
        }
    }

    public class SunTex : ModBloonCustomDisplay<Sun>
    {
        public override string AssetBundleName => "bosses";

        public override string PrefabName => "Heliosexual";

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var n in node.GetRenderers())
            {
                node.PrintInfo();
                n.ApplyOutlineShader();
                if(!n.name.ContainsIgnoreCase("body"))
                {
                    n.SetMainTexture(ModContent.GetTexture<AscendedBosses>("Sungodparts"));
                }
            }
            SetMeshOutlineColor(node, UnityEngine.Color.yellow);
        }
    }
    public class AscendedBlast : ModBloon
    {
        public override string BaseBloon => "Blastapopoulos1";
        public override string Icon => "AscendedBlast-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 8900000;

            HealthPercentTriggerModel health = bloonModel.GetBehavior<HealthPercentTriggerModel>();
            health.percentageValues = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };

            RangeReductionZoneModel RangeReductionZoneModel = bloonModel.GetBehavior<RangeReductionZoneModel>();
            RangeReductionZoneModel.rangeMultiplier = -0.20f;

            AbilityCooldownZoneModel abilityCooldownZoneModel = bloonModel.GetBehavior<AbilityCooldownZoneModel>();
            abilityCooldownZoneModel.multiplier = -0.15f;

            FireballActionModel FireballActionModel = bloonModel.GetBehavior<FireballActionModel>();
            FireballActionModel.projectileSpeed = 1f;
            FireballActionModel.stunDuration = 6f;
            FireballActionModel.magmaPoolRadius = 50;
            FireballActionModel.fireballAmount = 5;

            CreatePropsOnBloonActionModel CreatePropsOnBloonActionModel = bloonModel.GetBehavior<CreatePropsOnBloonActionModel>();
            CreatePropsOnBloonActionModel.rockAmount = 12;

            OverheatTriggerModel OverheatTriggerModel = bloonModel.GetBehavior<OverheatTriggerModel>();
            OverheatTriggerModel.heatEffectDuration = 25f;
            OverheatTriggerModel.immuneHeat *= 2f;
            OverheatTriggerModel.stunDuration = 6f;
        }
    }

    public class AscendedDread : ModBloon
    {
        public override string BaseBloon => "Dreadbloon4";
        public override string Icon => "AscendedDread-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 3100000;

            HealthPercentTriggerModel health = bloonModel.GetBehavior<HealthPercentTriggerModel>();
            health.percentageValues = new float[] { 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f};

            GenerateArmourActionModel Shield = bloonModel.GetBehavior<GenerateArmourActionModel>();
            Shield.amount = 5000;

            SpawnBloonsUntilArmourBreaksActionModel ArmorBreak = bloonModel.GetBehavior<SpawnBloonsUntilArmourBreaksActionModel>();
            ArmorBreak.timeBetweenSpawns = 2;
            ArmorBreak.initialSpawnPackSize = 6;
        }
    }

    public class Evil : ModBloon
    {
        public override string BaseBloon => "Bad";
        public override string Icon => "Evil-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 999999999;
            bloonModel.isBoss = true;
        }
    }

    public class Sun : ModBloon
    {
        public override string BaseBloon => "Bad";
        public override string Icon => "Sun-Icon";

        public override IEnumerable<string> DamageStates => [];
        public override void ModifyBaseBloonModel(BloonModel bloonModel)
        {
            bloonModel.RemoveAllChildren();
            bloonModel.maxHealth = 999999999;
            bloonModel.isBoss = true;
        }
    }

    public static T StartMonobehavior<T>() where T : MonoBehaviour
    { 
        var obj = InGame.instance.GetInGameUI().AddComponent<T>();

        return obj as T;
    }

    [HarmonyPatch(typeof(Bloon), nameof(Bloon.OnSpawn))]
    public class HandleBossSpawn_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Bloon __instance)
        {
            if(evilBuff)
            {
                BuffBloonSpeedModel buff = Game.instance.model.GetBloon("Vortex1").GetBehavior<BuffBloonSpeedModel>();
                buff.speedBoost = 0.5f;
                var mutator = buff.Mutator;
                __instance.AddMutator(mutator, 9999);
            }
            if(new System.Random().Next(500) == 0)
            {
                __instance.BecomeCursed();
            }
            if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Sun>())
            {
                FinalBossUI.SunBehavior mono = StartMonobehavior<FinalBossUI.SunBehavior>();

                mono.boss = __instance;
                HelpGoodBossUI.CreatePanel();
            }
            if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Evil>())
            {
                FinalBossUI.CreatePanel(__instance);
                FinalBossUI.EvilBehavior mono = StartMonobehavior<FinalBossUI.EvilBehavior>();

                mono.boss = __instance;
            }
            if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBloonarius>())
            {
                BossUI.CreatePanel(__instance);
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedVortex>())
            {
                BossUI.CreatePanel(__instance);
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedLych>())
            {
                BossUI.CreatePanel(__instance);
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedPhayze>())
            {
                BossUI.CreatePanel(__instance);
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBlast>())
            {
                BossUI.CreatePanel(__instance);
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedDread>())
            {
                BossUI.CreatePanel(__instance);
            }
        }
    }

    [HookTarget(typeof(BloonDamageHook), HookTargetAttribute.EHookType.Postfix)]
    [HookPriority(HookPriorityAttribute.Higher)]
    public static bool BloonDamagePostfix(Bloon @this, ref float totalAmount, Projectile projectile, ref bool distributeToChildren,ref bool overrideDistributeBlocker, ref bool createEffect, Tower tower, BloonProperties immuneBloonProperties,BloonProperties originalImmuneBloonProperties, ref bool canDestroyProjectile, ref bool ignoreNonTargetable, ref bool blockSpawnChildren, HookNullable<int> powerActivatedByPlayerId)
    {
        if (new System.Random().Next(0) == 0)
        {
            if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Sun>())
            {
                HelpGoodBossUI.orbs += 1;
            }
            if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Evil>())
            {
                HelpGoodBossUI.orbs += 1;
            }
        }
        if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Sun>())
        {
            FinalBossUI.HandleUI(@this, true);
        }
        else if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Evil>())
        {
            FinalBossUI.HandleUI(@this, false);
        }
        else if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBloonarius>())
        {
            BossUI.HandleUI(@this);
        }
        else if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedVortex>())
        {
            BossUI.HandleUI(@this);
        }
        else if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedLych>())
        {
            BossUI.HandleUI(@this);
        }
        else if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedPhayze>())
        {
            BossUI.HandleUI(@this);
        }
        else if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBlast>())
        {
            BossUI.HandleUI(@this);
        }
        else if (@this.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedDread>())
        {
            BossUI.HandleUI(@this);
        }

        return true;
    }

    [HarmonyPatch(typeof(Bloon), nameof(Bloon.OnDestroy))]
    public class HandleBossDestroy_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Bloon __instance)
        {
            if(__instance.HasTag("Cursed"))
            {
                GameObject lagFest = GameObject.Find("CursedOverlay" + __instance.Id);
                if(lagFest != null)
                {
                    lagFest.Destroy();
                }
            }
            if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Sun>())
            {
                FinalBossUI.instance.Close();
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.Evil>())
            {
                FinalBossUI.instance.Close();
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBloonarius>())
            {
                BossUI.instance.Close();
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedVortex>())
            {
                BossUI.instance.Close();
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedLych>())
            {
                BossUI.instance.Close();
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedPhayze>())
            {
                BossUI.instance.Close();
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedBlast>())
            {
                BossUI.instance.Close();
            }
            else if (__instance.bloonModel.baseId == ModContent.BloonID<AscendedBosses.AscendedDread>())
            {
                BossUI.instance.Close();
            }
        }
    }
}

public static class Ext
{
    public static void BecomeCursed(this Bloon bloon)
    {
        bloon.bloonModel.AddTag("Cursed");
        if(bloon.bloonModel.isMoab && !bloon.bloonModel.isBoss)
        {
            bloon.SetHealth(bloon.health * 2);
            bloon.bloonModel.speed *= 1.2f;
        }
        else
        {
            bloon.SetHealth(bloon.health * 2);
            bloon.bloonModel.speed *= 1.2f;
        }
    }
}
