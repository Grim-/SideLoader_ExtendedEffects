using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class ExtendedEffects : BaseUnityPlugin
    {
        public const string GUID = "sideloaderextendedeffects.extendedeffects";
        public const string NAME = "SideLoader Extemded Effects";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "1.0.0";

        // For accessing your BepInEx Logger from outside of this class (MyMod.Log)
        internal static ManualLogSource Log;

        //The name of the container for visual effects spawned by ExtendedEffects
        public const string SL_VISUAL_TRANSFORM = "SLVISUALCONTAINER";
        
        public static ConfigEntry<bool> AddTestItems;
        
        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;
            InitializeSL();
            InitializeConfig();
            new Harmony(GUID).PatchAll();
        }

        private void InitializeSL() {
            SL.BeforePacksLoaded += SL_BeforePacksLoaded;
        }
        
        private void InitializeConfig() {
            AddTestItems = Config.Bind(NAME, "Add Test Items", false, "Adds test items, spawnable using the debug mode menu (requires restart)");
        }

        private void SL_BeforePacksLoaded() {
            if (AddTestItems.Value) {
                DefineTestItems();
            }
        }

        private void DefineTestItems()
        {
            SL_MeleeWeapon TestWeapon = new SL_MeleeWeapon()
            {
                Target_ItemID = 2000031,
                New_ItemID = -69696969,
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
                             new SL_HidePlayer
                             {
                                 HideTime = 3f,
                             },
                             new SL_ForceAIState
                             {
                                 AIState = 0
                             }
                          },
                    }

                }
            };

            TestWeapon.ApplyTemplate();
        }

    }


}



//this is probably going to be needed some effects will explicity need to know who to target
//public enum SLEX_ConditionTargetType
//{
//    OWNER,
//    TARGET
//}
