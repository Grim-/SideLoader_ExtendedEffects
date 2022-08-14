using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
    public class SL_ApplyCharacterAuraVFX : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_ApplyCharacterAuraVFX);

        public Type GameModel => typeof(ApplyCharacterAuraVFX);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public float LifeTime;


        public override void ApplyToComponent<T>(T component)
        {
            ApplyCharacterAuraVFX comp = component as ApplyCharacterAuraVFX;
            comp.SLPackName = this.SLPackName;
            comp.AssetBundleName = this.AssetBundleName;
            comp.PrefabName = this.PrefabName;
            comp.LifeTime = this.LifeTime;
        }

        public override void SerializeEffect<T>(T effect)
        {
            ApplyCharacterAuraVFX comp = effect as ApplyCharacterAuraVFX;
            this.SLPackName = comp.SLPackName;
            this.AssetBundleName = comp.AssetBundleName;
            this.PrefabName = comp.PrefabName;
            this.LifeTime = comp.LifeTime;
        }
    }

    public class ApplyCharacterAuraVFX : Effect
    {
        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public float LifeTime;


        private GameObject Instance;
        private GameObject Prefab = null;
        private SkinnedMeshRenderer BodyMesh = null;
        private ParticleSystem[] CachedPS = null;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (Prefab == null)
            {
                Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);
            }


            if (Prefab != null)
            {
                if (BodyMesh == null)
                {
                    BodyMesh = FindBaseBody(_affectedCharacter);
                }
           
                if (BodyMesh == null)
                {
                    //cant find body mesh
                    ExtendedEffects.Instance.Log($"ApplyCharacterAuraVFX cant find body mesh");
                    return;
                }

                if (Instance == null)
                {
                    Instance = GameObject.Instantiate(Prefab);

                }

                Instance.transform.parent = _affectedCharacter.VisualHolderTrans;

                if (BodyMesh != null)
                {
                    UpdateVFXParticleSystem(Instance, BodyMesh);
                }

            }
            else
            {
                ExtendedEffects.Instance.Log($"ApplyCharacterAuraVFX Prefab is null.");
            }
      
        }
        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();

            if (Instance)
            {
                ExtendedEffects.Instance.Log($"ApplyCharacterAuraVFX Cleaning up Instance");
                GameObject.Destroy(Instance);
            }
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            if (Instance)
            {
                ExtendedEffects.Instance.Log($"ApplyCharacterAuraVFX destroying Instance");
                GameObject.Destroy(Instance);
            }
        }

        ///MBody0, MBody1 FBody0 etc
        private SkinnedMeshRenderer FindBaseBody(Character _affectedCharacter)
        {
            SkinnedMeshRenderer[] skinnedMeshRenderers = _affectedCharacter.VisualHolderTrans.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            foreach (var item in skinnedMeshRenderers)
            {
                if (item.transform.name.Contains("Body"))
                {
                    return item;
                }
            }

            return null;
        }

        private void UpdateVFXParticleSystem(GameObject VFXInstance, SkinnedMeshRenderer SkinnedMeshRenderer)
        {
            if (CachedPS == null)
            {
                CachedPS = VFXInstance.GetComponentsInChildren<ParticleSystem>();
            }

            if (CachedPS == null || CachedPS.Length == 0)
            {
                return;
            }

            foreach (var ps in CachedPS)
            {
                if (ps != null && ps.shape.shapeType == ParticleSystemShapeType.SkinnedMeshRenderer)
                {
                    ParticleSystem.ShapeModule shapeModule = ps.shape;
                    shapeModule.skinnedMeshRenderer = SkinnedMeshRenderer;
                    shapeModule.scale = Vector3.one;
                    shapeModule.position = Vector3.zero;
                    shapeModule.rotation = Vector3.zero;
                }
            }

            VFXInstance.transform.localPosition = Vector3.zero;
        }
    }
}
