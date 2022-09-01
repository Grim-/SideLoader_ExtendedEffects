using SideLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    //TODO
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

        private Dictionary<Renderer, Material> CurrentRenderers = new Dictionary<Renderer, Material>();

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            //only want to call this once
            if (!IsChanged)
            {
                Material TheMaterial = OutwardHelpers.GetFromAssetBundle<Material>(SLPackName, AssetBundleName, PrefabName);

              
      
                if (TheMaterial != null)
                {
                    CacheOriginalMaterials(_affectedCharacter);


                    ChangeMaterialForRenderers(TheMaterial);

                    //handle destroying if time set
                    if (ChangeTime > 0)
                    {
                        StartCoroutine(DelayRevert(_affectedCharacter));
                    }

                    IsChanged = true;
                }
            }

        }

        private IEnumerator DelayRevert(Character _affectedCharacter)
        {
            yield return new WaitForSeconds(ChangeTime);
            ReturnToOriginalMaterials();
            yield break;
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            base.StopAffectLocally(_affectedCharacter);

            if (ChangeTime <= 0)
            {
                ReturnToOriginalMaterials();
            }          
        }

        private void CacheOriginalMaterials(Character _affectedCharacter)
        {
            if (_affectedCharacter == null)
            {
                return;
            }

            //If the CurrentRenderers dict is populated then this has already run, return original references first
            if (CurrentRenderers.Count > 0)
            {
                ReturnToOriginalMaterials();
            }


            CurrentRenderers.Clear();

            ExtendedEffects.Instance.DebugLogMessage("Caching Original Materials");

            //cache all current renderers and their materials
            Renderer[] Renderers = GetRenderers(_affectedCharacter.VisualHolderTrans, false);

            foreach (var item in Renderers)
            {
                if (item != null)
                {
                    ExtendedEffects.Instance.DebugLogMessage($"Cached {item.transform.name} {item.material.name}");
                    CurrentRenderers.Add(item, new Material(item.material));
                }
            }

        }

        private void ChangeMaterialForRenderers(Material NewMaterial)
        {
            foreach (var item in CurrentRenderers)
            {
                if (item.Key == null || item.Value == null)
                {
                    continue;
                }

                if (CurrentRenderers.TryGetValue(item.Key, out Material Material))
                {
                    ExtendedEffects.Instance.DebugLogMessage($"Changing {item.Key.transform.name} to {NewMaterial}");
                    item.Key.material = NewMaterial;
                }

            }
        }

        private void ReturnToOriginalMaterials()
        {
            foreach (var item in CurrentRenderers)
            {
                if (item.Key == null || item.Value == null)
                {
                    continue;
                }
                else
                {
                    if (CurrentRenderers.TryGetValue(item.Key, out Material Material))
                    {
                        ExtendedEffects.Instance.DebugLogMessage($"Reverting {item.Key.transform.name} to {item.Value}");
                        item.Key.material = Material;
                    }
                }
            }

            IsChanged = false;
        }

        private Renderer[] GetRenderers(Transform transform, bool IncludeInActive)
        {
            return transform.GetComponentsInChildren<Renderer>(IncludeInActive);
        }
    }
}
