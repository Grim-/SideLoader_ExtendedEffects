using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    /// <summary>
    /// Spawn a VFX GameObject/Prefab from an AssetBundle and Attach it to the player - For boons and statuses and such.
    /// </summary>
    public class SL_PlayAssetBundleVFX : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_PlayAssetBundleVFX);
        public Type GameModel => typeof(SLEx_PlayAssetBundleVFX);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        public bool ParentToAffected;
        public float LifeTime;
       
        public override void ApplyToComponent<T>(T component)
        {
            SLEx_PlayAssetBundleVFX comp = component as SLEx_PlayAssetBundleVFX;
            comp.SLPackName = SLPackName;
            comp.AssetBundleName = AssetBundleName;
            comp.PrefabName = PrefabName;
            comp.PositionOffset = PositionOffset;
            comp.RotationOffset = RotationOffset;
            comp.ParentToAffected = ParentToAffected;
            comp.LifeTime = LifeTime;
        }

        //TODO : Clarify
        public override void SerializeEffect<T>(T effect)
        {
            SLEx_PlayAssetBundleVFX eff = effect as SLEx_PlayAssetBundleVFX;
            this.SLPackName = eff.SLPackName;
            this.AssetBundleName = eff.AssetBundleName;
            this.PrefabName = eff.PrefabName;
            this.PositionOffset = eff.PositionOffset;
            this.RotationOffset = eff.RotationOffset;
            this.ParentToAffected = eff.ParentToAffected;
            this.LifeTime = eff.LifeTime;
        }
    }

    public class SLEx_PlayAssetBundleVFX : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_PlayAssetBundleVFX);
        public Type GameModel => typeof(SLEx_PlayAssetBundleVFX);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        public bool ParentToAffected;
        public float LifeTime;

        private GameObject Instance;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            GameObject Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);

            if (Prefab != null)
            {
                if (Instance == null)
                {
                    Instance = GameObject.Instantiate(Prefab);
                }

                if (ParentToAffected)
                {
                    Instance.transform.parent = _affectedCharacter.VisualHolderTrans;
                    Instance.transform.localPosition = PositionOffset;
                    Instance.transform.localEulerAngles = RotationOffset;
                }
                else
                {
                    Instance.transform.position = _affectedCharacter.transform.position + PositionOffset;
                    Instance.transform.eulerAngles = RotationOffset;
                }


                if (LifeTime > 0)
                {
                    ExtendedEffects.Log($"SLEx_PlayAssetBundleVFX LIFETIME IS {LifeTime}");
                    Destroy(Instance, LifeTime);
                }
            }
            else
            {
                ExtendedEffects.Log($"SLEx_PlayAssetBundleVFX Prefab from AssetBundle {AssetBundleName} was null.");
            }            
        }

        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();

            if (LifeTime == 0)
            {
                ExtendedEffects.Log($"SLEx_PlayAssetBundleVFX CleanUpOnDestroy called and lifetime is 0, destroying.");
                if (Instance)
                {
                    GameObject.Destroy(Instance);
                }
            }


        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            if (LifeTime == 0)
            {
                ExtendedEffects.Log($"SLEx_PlayAssetBundleVFX StopAffectLocally called and lifetime is 0, destroying.");
                if (Instance)
                {
                    GameObject.Destroy(Instance);
                }
            }
        }
    }
}
