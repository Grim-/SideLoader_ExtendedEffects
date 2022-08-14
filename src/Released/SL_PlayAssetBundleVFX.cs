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
        public virtual Type SLTemplateModel => typeof(SL_PlayAssetBundleVFX);
        public virtual Type GameModel => typeof(SLEx_PlayAssetBundleVFX);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        public bool ParentToAffected;
        public bool PositionOffsetAsRelativeDirection;
        public bool RotateToPlayerDirection;
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
            comp.PositionOffsetAsRelativeDirection = PositionOffsetAsRelativeDirection;
            comp.RotateToPlayerDirection = RotateToPlayerDirection;
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
            this.RotateToPlayerDirection = eff.RotateToPlayerDirection;
            this.LifeTime = eff.LifeTime;
        }
    }

    public class SLEx_PlayAssetBundleVFX : Effect, ICustomModel
    {
        public virtual Type SLTemplateModel => typeof(SL_PlayAssetBundleVFX);
        public virtual Type GameModel => typeof(SLEx_PlayAssetBundleVFX);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        public bool ParentToAffected;
        public bool PositionOffsetAsRelativeDirection;
        public bool RotateToPlayerDirection;
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
                    //if the object is parented to the player already then 0,0,0 rotation is already player forward
                    Instance.transform.localEulerAngles = (RotateToPlayerDirection ?  Vector3.zero :  RotationOffset);
                }
                else
                {
                    Instance.transform.position = _affectedCharacter.transform.position + (PositionOffsetAsRelativeDirection ? _affectedCharacter.transform.forward + _affectedCharacter.transform.TransformVector(PositionOffset) : PositionOffset);
                    //world space rotation is to set whatever the players is, otherwise set to rotationoffset
                    Instance.transform.eulerAngles = (RotateToPlayerDirection ? _affectedCharacter.transform.eulerAngles : RotationOffset);
                }


                if (LifeTime > 0)
                {
                    ExtendedEffects.Instance.Log($"SLEx_PlayAssetBundleVFX LIFETIME IS {LifeTime}");
                    Destroy(Instance, LifeTime);
                }
            }
            else
            {
                ExtendedEffects.Instance.Log($"SLEx_PlayAssetBundleVFX Prefab from AssetBundle {AssetBundleName} was null.");
            }            
        }

        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();

            if (LifeTime == 0)
            {
                //ExtendedEffects.Instance.Log($"SLEx_PlayAssetBundleVFX CleanUpOnDestroy called and lifetime is 0, destroying.");
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
                //ExtendedEffects.Instance.Log($"SLEx_PlayAssetBundleVFX StopAffectLocally called and lifetime is 0, destroying.");
                if (Instance)
                {
                    GameObject.Destroy(Instance);
                }
            }
        }
    }

    //Side project for Alien
    //public class SL_ReplaceProjectileVFX : SL_Effect, ICustomModel
    //{
    //    public Type SLTemplateModel => typeof(SL_ReplaceProjectileVFX);

    //    public Type GameModel => typeof(SLEx_ReplaceProjectileVFX);

    //    public string SLPackName;
    //    public string AssetBundleName;
    //    public string PrefabName;
    //    public override void ApplyToComponent<T>(T component)
    //    {
    //        SLEx_ReplaceProjectileVFX comp = component as SLEx_ReplaceProjectileVFX;
    //        comp.SLPackName = SLPackName;
    //        comp.AssetBundleName = AssetBundleName;
    //        comp.PrefabName = PrefabName;
    //    }

    //    public override void SerializeEffect<T>(T effect)
    //    {
    //        SLEx_ReplaceProjectileVFX comp = effect as SLEx_ReplaceProjectileVFX;
    //        comp.SLPackName = SLPackName;
    //        comp.AssetBundleName = AssetBundleName;
    //        comp.PrefabName = PrefabName;
    //    }
    //}

    //public class SLEx_ReplaceProjectileVFX : Effect, ICustomModel
    //{
    //    public Type SLTemplateModel => typeof(SL_ReplaceProjectileVFX);

    //    public Type GameModel => typeof(SLEx_ReplaceProjectileVFX);


    //    public string SLPackName;
    //    public string AssetBundleName;
    //    public string PrefabName;

    //    public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
    //    {

    //        foreach (var item in this.m_parentSynchronizer.m_effects)
    //        {
    //            if (item.Value.Effect is ShootProjectile)
    //            {
    //                GameObject Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);
    //                Projectile projectile = Prefab.AddComponent<Projectile>();

    //                if (Prefab != null)
    //                {
    //                    (item.Value.Effect as ShootProjectile).BaseProjectile = projectile;
    //                }
    //            }
    //        }
            
    //    }

    //}
}
