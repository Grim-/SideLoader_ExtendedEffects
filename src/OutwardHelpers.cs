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

        public static T TryGetFromEquipmentItemVisual<T>(ItemVisual itemVisual)
        {
            foreach (var item in itemVisual.GetComponentsInChildren<BoxCollider>(false))
            {
                return item.GetComponentInChildren<T>();          
            }

            ExtendedEffects._Log.LogMessage($"Failed to find {typeof(T)} on ItemVisual {itemVisual.m_item.DisplayName}");
            return default(T);
        }

        public static bool DoesWeaponUseSkinnedMesh(Equipment weaponGameObject)
        {
            SkinnedMeshRenderer skinnedMesh = weaponGameObject.GetItemVisual().GetComponentInChildren<SkinnedMeshRenderer>();

            return skinnedMesh != null;
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

        public static Tag[] GetParentTags(this Effect effect) {
            Tag[] tags = {};
            EffectSynchronizer sync = effect.m_parentSynchronizer.SourceSynchronizer; // SourceSynchronizer is either the parent for subeffects, or itself for other synchronizers
            if (sync is Item)
            {
                tags = (sync as Item).Tags.ToArray();
            }
            if (sync is StatusEffect)
            {
                tags = (sync as StatusEffect).InheritedTags.ToArray();
            }
            return tags;
        }
        public static Tag[] GetParentTags(this EffectCondition condition) {
            Tag[] tags = {};
            EffectSynchronizer sync = condition.m_parentSynchronizer.SourceSynchronizer; // SourceSynchronizer is either the parent for subeffects, or itself for other synchronizers
            if (sync is Item)
            {
                tags = (sync as Item).Tags.ToArray();
            }
            if (sync is StatusEffect)
            {
                tags = (sync as StatusEffect).InheritedTags.ToArray();
            }
            return tags;
        }

        public static float GetTaggedStat(this Character character, Tag stat, Tag[] tags) {
            return character.Stats.GetStat(stat).GetValue(tags);
        }
        public static float GetTaggedStatModifier(this Character character, Tag stat, Tag[] tags) {
            return character.Stats.GetStat(stat).GetModifier(tags) * 100;
        }
    }

    public enum Attributes
    {
        HEALTH,
        MANA,
        STAMINA,
        BURNT_HEALTH,
        BURNT_MANA,
        BURNT_STAMINA
    }
    public enum DamageSourceType {
        ANY,
        WEAPON,
        NON_WEAPON
    }
}