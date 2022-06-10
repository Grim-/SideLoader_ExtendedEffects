//using SideLoader;
//using System;
//using UnityEngine;

//public class SL_SpawnPrefabOnWeapon : SL_Effect, ICustomModel
//{
//    public Type SLTemplateModel => typeof(SL_SpawnPrefabOnWeapon);

//    public Type GameModel => typeof(SLEx_SpawnPrefabOnWeapon);

//    public string SLPackName;
//    public string AssetBundle;
//    public string AssetName;

//    public float DestroyDelay = 0;


//    public override void ApplyToComponent<T>(T component)
//    {
//        SLEx_SpawnPrefabOnWeapon SLEx_SpawnPrefabOnWeapon = component as SLEx_SpawnPrefabOnWeapon;

//        SLEx_SpawnPrefabOnWeapon.SLPackName = SLPackName;
//        SLEx_SpawnPrefabOnWeapon.AssetBundle = AssetBundle;
//        SLEx_SpawnPrefabOnWeapon.AssetName = AssetName;
//        SLEx_SpawnPrefabOnWeapon.DestroyDelay = DestroyDelay;
//    }

//    public override void SerializeEffect<T>(T effect)
//    {
       
//    }
//}

//public class SLEx_SpawnPrefabOnWeapon : Effect, ICustomModel
//{
//    public Type SLTemplateModel => typeof(SL_SpawnPrefabOnWeapon);

//    public Type GameModel => typeof(SLEx_SpawnPrefabOnWeapon);

//    public string SLPackName;
//    public string AssetBundle;
//    public string AssetName;

//    public float DestroyDelay;

//    public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
//    {
//       // this.OwnerCharacter.PlayVisualsVFX(0);
//        SideLoader_ExtendedEffects.ExtendedEffects.Log.LogMessage($"SL_SpawnPrefabOnWeapon  SL PACK : {SLPackName} BUNDLE NAME : {AssetBundle} ASSET NAME :{AssetName}");

//        GameObject PrefabReference = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundle, AssetName);

//        if (PrefabReference != null)
//        {
           
//            if (this.OwnerCharacter.CurrentWeapon != null)
//            {
//                Renderer WeaponRenderer = OutwardHelpers.TryGetWeaponRenderer(this.OwnerCharacter.CurrentWeapon);

//                if (WeaponRenderer != null)
//                {
//                    GameObject SLVisualContainer = CreateVisualContainerIfNoExists();
//                    DestroyVisualTransformChildIfExists(SLVisualContainer, PrefabReference.name);
//                    CreatePrefabInstanceAndParent(PrefabReference, SLVisualContainer);

//                }
//                else
//                {
//                    SideLoader_ExtendedEffects.ExtendedEffects.Log.LogMessage($"SL_SpawnPrefabOnWeapon : Could not find Weapon Renderer.");
//                }
//            }
//        }
//        else
//        {
//            SideLoader_ExtendedEffects.ExtendedEffects.Log.LogMessage($"SL_SpawnPrefabOnWeapon : Could not find Asset");
//        }

//    }


//    private GameObject CreateVisualContainerIfNoExists()
//    {
//        GameObject SLVisualContainer = null;

//        for (int i = 0; i < this.OwnerCharacter.CurrentWeapon.LoadedVisual.transform.childCount; i++)
//        {
//            if (this.OwnerCharacter.CurrentWeapon.LoadedVisual.transform.GetChild(i).name == SideLoader_ExtendedEffects.ExtendedEffects.SL_VISUAL_TRANSFORM)
//            {
//                SLVisualContainer = this.OwnerCharacter.CurrentWeapon.LoadedVisual.transform.GetChild(i).gameObject;
//            }
//        }

//        if (SLVisualContainer == null)
//        {
//            SLVisualContainer = CreateVisualContainer();
//        }

//        return SLVisualContainer;
//    }

//    private GameObject CreateVisualContainer()
//    {
//        GameObject SLVisualContainer = new GameObject();
//        SLVisualContainer.name = SideLoader_ExtendedEffects.ExtendedEffects.SL_VISUAL_TRANSFORM;
//        SLVisualContainer.transform.parent = this.OwnerCharacter.CurrentWeapon.LoadedVisual.transform;
//        SLVisualContainer.transform.localPosition = Vector3.zero;
//        return SLVisualContainer;
//    }

//    private void DestroyVisualTransformChildIfExists(GameObject VisualContainer, string PrefabReferenceName)
//    {

//        for (int i = 0; i < VisualContainer.transform.childCount; i++)
//        {
//            if (VisualContainer.transform.GetChild(i).name == PrefabReferenceName)
//            {
//                SideLoader_ExtendedEffects.ExtendedEffects.Log.LogMessage($"SL_SpawnPrefabAndParentToWeapon : Prefab to be Spawned already exists on SL VISUAL TRANSFORM, Destroying.");
//                Destroy(VisualContainer.transform.GetChild(i).gameObject);
//                break;
//            }

//        }
//    }

//    private GameObject CreatePrefabInstanceAndParent(GameObject PrefabReference, GameObject VisualContainer)
//    {
//        GameObject PrefabInstance = GameObject.Instantiate(PrefabReference);
//        PrefabInstance.transform.parent = VisualContainer.transform;
//        PrefabInstance.transform.name = PrefabReference.name;
//        PrefabInstance.transform.localPosition = Vector3.zero;
//        PrefabInstance.transform.localEulerAngles = Vector3.zero;

//        if (DestroyDelay > 0)
//        {
//            Destroy(PrefabInstance, DestroyDelay);
//        }

//        return PrefabInstance;
//    }

//}
