using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using SideLoader.SaveData;
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
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "1.1.4";

        // For accessing your BepInEx Logger from outside of this class (MyMod.Log)
        internal static ManualLogSource _Log;

        //The name of the container for visual effects spawned by ExtendedEffects
        public const string SL_VISUAL_TRANSFORM = "SLVISUALCONTAINER";

        public static ConfigEntry<bool> AddTestItems;
        public static ConfigEntry<bool> ShowDebugLog;
        //ID RANGE -26000 -> -26999

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            _Log = this.Logger;
            InitializeSL();
            InitializeConfig();
            new Harmony(GUID).PatchAll();
        }


        private void InitializeSL()
        {
            SL.BeforePacksLoaded += SL_BeforePacksLoaded;
        }

        private void InitializeConfig()
        {
            AddTestItems = Config.Bind(NAME, "Add Test Items", false, "Adds test items, spawnable using the debug mode menu (requires restart)");
            ShowDebugLog = Config.Bind(NAME, "Show Debug Log", false, "Enables the Debug Log for SideLoader Extended Effects.");
        }

        private void SL_BeforePacksLoaded()
        {
            if (AddTestItems.Value)
            {
                DefineTestItems();
            }
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
                             //new SL_PlayAssetBundleVFX
                             //{
                             //    SLPackName = "EmosUniques_SeekingStone",
                             //    AssetBundleName = "emoseekingstone",
                             //    PrefabName = "SeekingStone",
                             //    LifeTime = 10
                             //}
                          },
                    }

                }
            };
            TestWeapon.ApplyTemplate();


            //SL_Item TestImbueLifeWardenEffectPotion = new SL_Item()
            //{
            //    Target_ItemID = 4300130,
            //    New_ItemID = -26987,
            //    Name = "Test Apply Enchantment Potion",
            //    Description = "Test Apply Enchantment Potion",
            //    EffectBehaviour = EditBehaviours.Destroy,
            //    QtyRemovedOnUse = 0,
            //    EffectTransforms = new SL_EffectTransform[] {
            //        new SL_EffectTransform {
            //            TransformName = "Effects",
            //            Effects = new SL_Effect[]
            //            {
            //                   new SL_ApplyEnchantment
            //                   {
            //                       EquipmentSlot = EquipmentSlot.EquipmentSlotIDs.RightHand,
            //                       EnchantmentID = 34,
            //                       ApplyPermanently = true
            //                   }
            //            }
            //        }
            //    }
            //};
            //TestImbueLifeWardenEffectPotion.ApplyTemplate();

            //SL_StatusEffect TestStatusBlackThunder = new SL_StatusEffect()
            //{
            //    TargetStatusIdentifier = "Discipline",
            //    NewStatusID = -26233,
            //    StatusIdentifier = "CustomImbueBlackThunder",
            //    Name = "CustomImbue Test",
            //    Description = "CustomImbueBlackThunder",
            //    Purgeable = false,
            //    DisplayedInHUD = true,
            //    IsMalusEffect = false,
            //    Lifespan = 60,
            //    RefreshRate = -1f,
            //    AmplifiedStatusIdentifier = string.Empty,
            //    FamilyMode = StatusEffect.FamilyModes.Bind,
            //    EffectBehaviour = EditBehaviours.Destroy,
            //    Effects = new SL_EffectTransform[] {
            //        new SL_EffectTransform {
            //            TransformName = "Effects",
            //            Effects = new SL_Effect[] {
            //                new SL_CustomImbueVFX
            //                {
            //                    SLPackName = "vfx",
            //                    AssetBundleName = "emovfx",
            //                    PrefabName = "Black Imbue",
            //                    IsMainHand = true
            //                }
            //            }
            //        }
            //    }
            //};
            //TestStatusBlackThunder.ApplyTemplate();

            //SL_Item TestImbueEffectPotion = new SL_Item()
            //{
            //    Target_ItemID = 4300130,
            //    New_ItemID = -26986,
            //    Name = "Test Imbue Black Thundern Effect Potion",
            //    Description = "Test Imbue Black Thundern Effect Potion",
            //    EffectBehaviour = EditBehaviours.Destroy,
            //    QtyRemovedOnUse = 0,
            //    EffectTransforms = new SL_EffectTransform[]
            //    {
            //        new SL_EffectTransform
            //        {
            //            TransformName = "Effects",
            //            Effects = new SL_Effect[]
            //            {
            //                new SL_AddStatusEffect
            //                {
            //                    StatusEffect = "CustomImbueBlackThunder"
            //                }
            //            }
            //        }
            //    }
            //};
            //TestImbueEffectPotion.ApplyTemplate();

            SL_StatusEffect TestSuspendStatusEffect = new SL_StatusEffect()
            {
                TargetStatusIdentifier = "Discipline",
                NewStatusID = -26998,
                StatusIdentifier = "SuspendEffectTest",
                Name = "Suspend Effect Test",
                Description = "SuspendEffectTest",
                Purgeable = true,
                DisplayedInHUD = true,
                IsMalusEffect = false,
                Lifespan = 60,
                RefreshRate = 1f,
                AmplifiedStatusIdentifier = string.Empty,
                FamilyMode = StatusEffect.FamilyModes.Bind,
                EffectBehaviour = EditBehaviours.Destroy,
                Effects = new SL_EffectTransform[] {
                    new SL_EffectTransform {
                        TransformName = "Effects",
                        Effects = new SL_Effect[] {
                            new SL_SuspendStatusEffect() {
                                StatusEffectIdentifiers = new []{"Poisoned", "Bleeding", "Hallowed Marsh Poison Lvl1", "Hallowed Marsh Poison Lvl2"}
                            },
                            new SL_SuspendStatusTimer() {
                                StatusEffectIdentifiers = new []{"Poisoned", "Bleeding", "Hallowed Marsh Poison Lvl1", "Hallowed Marsh Poison Lvl2"}
                            }
                        }
                    }
                }
            };
            TestSuspendStatusEffect.ApplyTemplate();

            SL_Item TestSuspendEffectPotion = new SL_Item()
            {
                Target_ItemID = 4300130,
                New_ItemID = -26997,
                Name = "Test Suspend Effect Potion",
                Description = "Test",
                EffectBehaviour = EditBehaviours.Destroy,
                EffectTransforms = new SL_EffectTransform[] {
                    new SL_EffectTransform {
                        TransformName = "Effects",
                        Effects = new SL_Effect[] {
                            new SL_AddStatusEffect {
                                StatusEffect = "SuspendEffectTest"
                            }
                        }
                    }
                }
            };
            TestSuspendEffectPotion.ApplyTemplate();
        }


        public static void Log(string logMessage)
        {
            if (ShowDebugLog.Value)
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