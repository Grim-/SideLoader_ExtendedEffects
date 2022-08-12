using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using SideLoader.SaveData;
using SideLoader_ExtendedEffects.Item_Context_Menu;
using SideLoader_ExtendedEffects.Released;
using SideLoader_ExtendedEffects.Released.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SideLoader_ExtendedEffects
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class ExtendedEffects : BaseUnityPlugin
    {
        public const string GUID = "sideloaderextendedeffects.extendedeffects";
        public const string NAME = "SideLoader Extended Effects";

        public const string VERSION = "1.2.0";

        // For accessing your BepInEx Logger from outside of this class (MyMod.Log)
        internal static ManualLogSource _Log;

        //The name of the container for visual effects spawned by ExtendedEffects
        public const string SL_VISUAL_TRANSFORM = "SLVISUALCONTAINER";

        public static ConfigEntry<bool> AddTestItems;
        public static ConfigEntry<bool> ShowDebugLog;


        public static ExtendedEffects Instance { get; private set; }

        public CustomItemMenuManager CustomItemMenuManager { get; private set; }
        

        /// <summary>
        /// Called when ExtendedEffects has fully loaded.
        /// </summary>
        public Action OnReady;

        //ID RANGE -26000 -> -26999

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            _Log = this.Logger;
            Instance = this;
            CustomItemMenuManager = new CustomItemMenuManager();

            InitializeSL();
            InitializeConfig();
            CustomMenuItemTest();
            new Harmony(GUID).PatchAll();
            OnReady?.Invoke();
        }


        private void InitializeSL()
        {
            SL.BeforePacksLoaded += SL_BeforePacksLoaded;
        }

        private void InitializeConfig()
        {
            AddTestItems = Config.Bind(NAME, "Add Test Items", false, "Adds test items, spawnable using the debug mode menu (requires restart)");
            ShowDebugLog = Config.Bind(NAME, "Show Debug Log", true, "Enables the Debug Log for SideLoader Extended Effects.");
        }

        private void SL_BeforePacksLoaded()
        {
            if (AddTestItems.Value)
            {
                DefineTestItems();
            }
        }

        private void CustomMenuItemTest()
        {
            ////Test for ALL items
            //CustomItemMenuManager.RegisterCustomMenuOption(101010, "TEST STRING", (Character, Item, ItemDisplayOptionPanel, someInt) =>
            //{
            //    Character.CharacterUI.ShowInfoNotification($"I have clicked the thing! {Item.DisplayName}");
            //},
            //null);

            ////Add a custom action that only shows up when the item clicked is of the ID 2100110, show a slightly different notification
            //CustomItemMenuManager.RegisterCustomMenuOption(101110, "Test Sword", (Character, Item, ItemDisplayOptionPanel, someInt) =>
            //{
            //    Character.CharacterUI.ShowInfoNotification($"I have clicked the Assasins Claymore thing! {Item.DisplayName}");
            //},
            //(Character, Item, ItemDisplayOptionPanel, someInt) =>
            //{
            //    if (Item.ItemID == 2100110)
            //    {
            //        return true;
            //    }
            //    return false;
            //});
        }

        private void DefineTestItems()
        {
            SL_MeleeWeapon TestWeapon = new SL_MeleeWeapon()
            {
                Target_ItemID = 2000031,
                New_ItemID = -26999,
                Name = "Emo Test Blade of Testing",
                Description = "WHACK",
                StatsHolder = new SL_WeaponStats()
                {
                    BaseDamage = new List<SL_Damage>()
                    {
                        new SL_Damage()
                        {
                            Damage = 25f,
                            Type = DamageType.Types.Physical
                        },
                    },
                    AttackSpeed = 0.9f
                },
                EffectTransforms = new SL_EffectTransform[]
                {
                    new SL_EffectTransform
                    {
                        TransformName = "HitEffects",
                        Effects = new SL_Effect[]
                        {

                        }
                    }

                }
            };
            TestWeapon.ApplyTemplate();

            SL_Item TestPotion = new SL_Item()
            {
                Target_ItemID = 4300130,
                New_ItemID = -26987,
                Name = "Emo Test Potion",
                Description = "Test Potion",
                EffectBehaviour = EditBehaviours.Destroy,
                QtyRemovedOnUse = 0,
                EffectTransforms = new SL_EffectTransform[]
                {
                    new SL_EffectTransform
                    {
                        TransformName = "Effects",
                        Effects = new SL_Effect[]
                        {

                        }
                    }
                }
            };
            TestPotion.ApplyTemplate();



            SL_Skill TestSkill = new SL_Skill()
            {
                Target_ItemID = 8100120,
                New_ItemID = -26986,
                Name = "Emo Test Skill",
                EffectBehaviour = EditBehaviours.OverrideEffects,
                EffectTransforms = new SL_EffectTransform[]
                {
                    new SL_EffectTransform
                    {
                        TransformName = "RageActivation",
                        Effects = new SL_Effect[]
                        {
                            new SL_AddStatusEffect()
                            {
                                StatusEffect = "Rage",
                            }
                        },
                        EffectConditions = new SL_EffectCondition[]
                        {
                            new SL_IsTimeBetweenCondition()
                            {
                                StartHour = 1,
                                EndHour = 10
                            }
                        }
                    },
                }
            };

            TestSkill.ApplyTemplate();
        }

        public void Log(object logMessage)
        {
            if (ShowDebugLog.Value && _Log != null)
            {
                _Log.LogMessage(logMessage);
            }
        }
    }
}
//human bone ids
//Hips = 0,
//LeftUpperLeg = 1,
//RightUpperLeg = 2,
//LeftLowerLeg = 3,
//RightLowerLeg = 4,
//LeftFoot = 5,
//RightFoot = 6,
//Spine = 7,
//Chest = 8,
//Neck = 9,
//Head = 10,
//LeftShoulder = 11,
//RightShoulder = 12,
//LeftUpperArm = 13,
//RightUpperArm = 14,
//LeftLowerArm = 15,
//RightLowerArm = 16,
//LeftHand = 17,
//RightHand = 18,
//LeftToes = 19,
//RightToes = 20,
//LeftEye = 21,
//RightEye = 22,
//Jaw = 23,
//LeftThumbProximal = 24,
//LeftThumbIntermediate = 25,
//LeftThumbDistal = 26,
//LeftIndexProximal = 27,
//LeftIndexIntermediate = 28,
//LeftIndexDistal = 29,
//LeftMiddleProximal = 30,
//LeftMiddleIntermediate = 31,
//LeftMiddleDistal = 32,
//LeftRingProximal = 33,
//LeftRingIntermediate = 34,
//LeftRingDistal = 35,
//LeftLittleProximal = 36,
//LeftLittleIntermediate = 37,
//LeftLittleDistal = 38,
//RightThumbProximal = 39,
//RightThumbIntermediate = 40,
//RightThumbDistal = 41,
//RightIndexProximal = 42,
//RightIndexIntermediate = 43,
//RightIndexDistal = 44,
//RightMiddleProximal = 45,
//RightMiddleIntermediate = 46,
//RightMiddleDistal = 47,
//RightRingProximal = 48,
//RightRingIntermediate = 49,
//RightRingDistal = 50,
//RightLittleProximal = 51,
//RightLittleIntermediate = 52,
//RightLittleDistal = 53,
//UpperChest = 54,
//LastBone = 55