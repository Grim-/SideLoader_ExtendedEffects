using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    public class SL_PlayAssetBundleVFX_Bones : SL_PlayAssetBundleVFX
    {
        public override Type SLTemplateModel => typeof(SL_PlayAssetBundleVFX_Bones);
        public override Type GameModel => typeof(SLEx_PlayAssetBundleVFX_Bones);

        public int BoneID;

        public override void ApplyToComponent<T>(T component)
        {
            SLEx_PlayAssetBundleVFX_Bones comp = component as SLEx_PlayAssetBundleVFX_Bones;
            comp.SLPackName = SLPackName;
            comp.AssetBundleName = AssetBundleName;
            comp.PrefabName = PrefabName;
            comp.PositionOffset = PositionOffset;
            comp.RotationOffset = RotationOffset;
            comp.ParentToAffected = ParentToAffected;
            comp.PositionOffsetAsRelativeDirection = PositionOffsetAsRelativeDirection;
            comp.LifeTime = LifeTime;
            comp.RotateToPlayerDirection = RotateToPlayerDirection;
            comp.BoneID = BoneID;
        }

        public override void SerializeEffect<T>(T effect)
        {
            SLEx_PlayAssetBundleVFX_Bones comp = effect as SLEx_PlayAssetBundleVFX_Bones;
            this.SLPackName = comp.SLPackName;
            this.AssetBundleName = comp.AssetBundleName;
            this.PrefabName = comp.PrefabName;
            this.PositionOffset = comp.PositionOffset;
            this.RotationOffset = comp.RotationOffset;
            this.ParentToAffected = comp.ParentToAffected;
            this.PositionOffsetAsRelativeDirection = comp.PositionOffsetAsRelativeDirection;
            this.LifeTime = comp.LifeTime;
            this.BoneID = comp.BoneID;
        }
    }

    public class SLEx_PlayAssetBundleVFX_Bones : SLEx_PlayAssetBundleVFX
    {
        public override Type SLTemplateModel => typeof(SL_PlayAssetBundleVFX_Bones);
        public override Type GameModel => typeof(SLEx_PlayAssetBundleVFX_Bones);

        public int BoneID;

        private GameObject Instance;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            GameObject Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);

            if (Prefab != null)
            {
                Transform SelectedBone = OutwardHelpers.TryGetHumanBone(_affectedCharacter, (HumanBodyBones)BoneID);

                if (SelectedBone == null)
                {
                    ExtendedEffects.Instance.DebugLogMessage($"SLEx_PlayAssetBundleVFX_Bone cannot find target bone");
                    return;
                }

                if (Instance == null)
                {
                    Instance = GameObject.Instantiate(Prefab);
                }


                if (ParentToAffected)
                {
                    Instance.transform.parent = SelectedBone;
                    Instance.transform.localPosition = PositionOffset;
                    Instance.transform.localEulerAngles = RotationOffset;
                }
                else
                {
                    Instance.transform.position = SelectedBone.transform.position + (PositionOffsetAsRelativeDirection ? _affectedCharacter.transform.TransformPoint(PositionOffset) : PositionOffset);
                    Instance.transform.eulerAngles = (RotateToPlayerDirection ? _affectedCharacter.transform.eulerAngles : RotationOffset);
                }


                if (LifeTime > 0)
                {
                    ExtendedEffects.Instance.DebugLogMessage($"SLEx_PlayAssetBundleVFX_Bones LIFETIME IS {LifeTime}");
                    Destroy(Instance, LifeTime);
                }
            }
            else
            {
                ExtendedEffects.Instance.DebugLogMessage($"SLEx_PlayAssetBundleVFX_Bones Prefab from AssetBundle {AssetBundleName} was null.");
            }
        }

        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();

            if (LifeTime == 0)
            {
                //ExtendedEffects.Instance.Log($"SLEx_PlayAssetBundleVFX_Bones CleanUpOnDestroy called and lifetime is 0, destroying.");
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
                //ExtendedEffects.Instance.Log($"SLEx_PlayAssetBundleVFX_Bones StopAffectLocally called and lifetime is 0, destroying.");
                if (Instance)
                {
                    GameObject.Destroy(Instance);
                }
            }
        }
    }
}

