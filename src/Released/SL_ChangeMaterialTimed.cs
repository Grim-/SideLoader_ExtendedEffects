using SideLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    public class SL_ChangeMaterialTimed : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_ChangeMaterialTimed);
        public Type GameModel => typeof(ChangeMaterialTimed);


        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public float ChangeTime;


        public override void ApplyToComponent<T>(T component)
        {
            ChangeMaterialTimed SL_ChangeMaterialForTime = component as ChangeMaterialTimed;
            SL_ChangeMaterialForTime.ChangeTime = ChangeTime;
            SL_ChangeMaterialForTime.SLPackName = SLPackName;
            SL_ChangeMaterialForTime.AssetBundleName = AssetBundleName;
            SL_ChangeMaterialForTime.PrefabName = PrefabName;
        }

        public override void SerializeEffect<T>(T effect)
        {
            ChangeMaterialTimed SL_ChangeMaterialForTime = effect as ChangeMaterialTimed;
            this.ChangeTime = SL_ChangeMaterialForTime.ChangeTime;
            this.SLPackName = SL_ChangeMaterialForTime.SLPackName;
            this.AssetBundleName = SL_ChangeMaterialForTime.AssetBundleName;
            this.PrefabName = SL_ChangeMaterialForTime.PrefabName;
        }
    }

    ///TODO : Custom base class for Effects that make use of coroutines, otherwise depending on the context the effect is called in, it could start multiple a frame

    public class ChangeMaterialTimed : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_ChangeMaterialTimed);
        public Type GameModel => typeof(ChangeMaterialTimed);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public float ChangeTime;

        private bool IsChanged;

        private Dictionary<Renderer, Material> OriginalMaterials = new Dictionary<Renderer, Material>();

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            //only want to call this once
            if (!IsChanged)
            {
                CacheOriginalMaterials(_affectedCharacter);

                Material TheMaterial = OutwardHelpers.GetFromAssetBundle<Material>(SLPackName, AssetBundleName, PrefabName);

                if (TheMaterial != null)
                {
                    ChangeAllMaterials(_affectedCharacter, TheMaterial);
                }


                //handle destroying if time set
                if (ChangeTime > 0)
                {
                    StartCoroutine(DelayRevert(_affectedCharacter));
                }

                IsChanged = true;
            }

        }

        private IEnumerator DelayRevert(Character _affectedCharacter)
        {
            yield return new WaitForSeconds(ChangeTime);
            ReturnToOriginalMaterials(_affectedCharacter);
            yield break;
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            base.StopAffectLocally(_affectedCharacter);

            //not handled by coroutine
            if (ChangeTime <= 0)
            {
                ReturnToOriginalMaterials(_affectedCharacter);
            }          
        }

        private void CacheOriginalMaterials(Character _affectedCharacter)
        {
            ExtendedEffects.Log("Cacheing Original Materials");
            OriginalMaterials = new Dictionary<Renderer, Material>();

            Renderer[] Renderers = GetRenderers(_affectedCharacter.VisualHolderTrans);

            foreach (var item in Renderers)
            {
                if (item != null && item.material != null)
                {
                    ExtendedEffects.Log($"Cached {item.transform.name} {item.material.name}");
                    OriginalMaterials.Add(item, item.material);
                }
            }

        }

        private void ChangeAllMaterials(Character _affectCharacter, Material NewMaterial)
        {
            Renderer[] Renderers = GetRenderers(_affectCharacter.VisualHolderTrans);

            foreach (var item in Renderers)
            {
                ExtendedEffects.Log($"Changing {item.transform.name} to {NewMaterial}");
                item.material = NewMaterial; 
            }
        }

        private void ReturnToOriginalMaterials(Character _affectedCharacter)
        {
            Renderer[] Renderers = GetRenderers(_affectedCharacter.VisualHolderTrans);

            foreach (var item in Renderers)
            {
                if (OriginalMaterials.ContainsKey(item) && OriginalMaterials[item] != null)
                {
                    ExtendedEffects.Log($"Reverting {item.transform.name} to {OriginalMaterials[item]}");
                    item.material = OriginalMaterials[item];
                }
            }
        }

        private Renderer[] GetRenderers(Transform transform)
        {
            return transform.GetComponentsInChildren<Renderer>();
        }
    }
}
