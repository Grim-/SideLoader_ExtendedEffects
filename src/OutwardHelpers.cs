using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    public static class OutwardHelpers
    {
        public static string Emission_Property_Value = "_EmissionColor";

        public static void UpdateWeaponDamage(Weapon WeaponInstance, DamageList newDamageList)
        {
            WeaponInstance.Damage.Clear();
            //just fucken nuke everything 
            WeaponInstance.Stats.BaseDamage = newDamageList;
            WeaponInstance.m_baseDamage = WeaponInstance.Stats.BaseDamage.Clone();
            WeaponInstance.m_activeBaseDamage = WeaponInstance.Stats.BaseDamage.Clone();
            WeaponInstance.baseDamage = WeaponInstance.Stats.BaseDamage.Clone();
            WeaponInstance.Stats.Attacks = SL_WeaponStats.GetScaledAttackData(WeaponInstance);
            //ta-da updated weapon damage
        }

        public static MeshFilter TryGetWeaponMesh(Equipment weaponGameObject, bool IncludeInActive = true)
        {
            foreach (var item in weaponGameObject.LoadedVisual.GetComponentsInChildren<BoxCollider>(true))
            {
                ExtendedEffects.Instance.DebugLogMessage($"BoxCollider GO Name {item.transform.name}");

                if (item.transform.parent.gameObject.activeInHierarchy)
                {
                    return item.GetComponent<MeshFilter>();
                }

            }
            return null;
        }

        public static T TryGetWeaponVisualComponent<T>(Equipment weaponGameObject, bool IncludeInActive = true)
        {
            return weaponGameObject.LoadedVisual.GetComponentInChildren<BoxCollider>(IncludeInActive).GetComponent<T>();
        }

        public static Transform TryGetHumanBone(Character character, HumanBodyBones bone)
        {
            return character.Animator != null ? character.Animator.GetBoneTransform(bone) : null;
        }

        public static T GetFromAssetBundle<T>(string SLPackName, string AssetBundle, string key) where T : UnityEngine.Object
        {
            if (!SL.PacksLoaded)
            {
                return default(T);
            }

            return SL.GetSLPack(SLPackName).AssetBundles[AssetBundle].LoadAsset<T>(key);
        }


        public static List<T> GetTypeFromColliders<T>(Collider[] colliders) where T : Component
        {
            List<T> list = new List<T>();
            foreach (var col in colliders)
            {
                T type = col.GetComponentInChildren<T>();
                if (type != null)
                {
                    list.Add(type);
                }
            }
            return list;
        }

        public static Tag GetTagFromName(string tagName)
        {
            //ExtendedEffects.Instance.DebugLogMessage($"Getting tag {tagName}");

            foreach (var tag in TagSourceManager.Instance.m_tags)
            {
                //ExtendedEffects.Instance.DebugLogMessage($"Tag {tag}");

                if (tag.TagName == tagName)
                {
                    //ExtendedEffects.Instance.DebugLogMessage($"Tag {tag} matches {tagName}");
                    return tag;
                }
            }

            //ExtendedEffects.Instance.DebugLogMessage($"No Tag found with name {tagName}");
            return default(Tag);
        }
    }
}