using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects
{
    public class SL_CustomImbueVFX : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_CustomImbueVFX);
        public Type GameModel => typeof(SLEx_CustomImbueVFX);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;

        public Vector3 PositionOffset;
        public Vector3 RotationOffset;

        public bool IsMainHand = true;


        public override void ApplyToComponent<T>(T component)
        {
            SLEx_CustomImbueVFX comp = component as SLEx_CustomImbueVFX;

            comp.SLPackName = SLPackName;
            comp.AssetBundleName = AssetBundleName;
            comp.PrefabName = PrefabName;
            comp.PositionOffset = PositionOffset;
            comp.RotationOffset = RotationOffset;
            comp.IsMainHand = IsMainHand;
        }

        public override void SerializeEffect<T>(T effect)
        {
            SLEx_CustomImbueVFX comp = effect as SLEx_CustomImbueVFX;

            this.SLPackName = comp.SLPackName;
            this.AssetBundleName = comp.AssetBundleName;
            this.PrefabName = comp.PrefabName;
            this.PositionOffset = comp.PositionOffset;
            this.RotationOffset = comp.RotationOffset;
            this.IsMainHand = comp.IsMainHand;
        }
    }

    public class SLEx_CustomImbueVFX : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_CustomImbueVFX);
        public Type GameModel => typeof(SLEx_CustomImbueVFX);

        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        public bool IsMainHand;

        private GameObject Instance;
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            ExtendedEffects.Log($"SLEx_CustomImbueVFX");

            EquipmentSlot OffHand = _affectedCharacter.Inventory.Equipment.GetMatchingSlot(EquipmentSlot.EquipmentSlotIDs.LeftHand);
            Equipment CurrentWeapon = null;

            if (IsMainHand)
            {
                ExtendedEffects.Log($"SLEx_CustomImbueVFX Target is MainHand");
                CurrentWeapon = _affectedCharacter.CurrentWeapon;

                if (_affectedCharacter.CurrentWeapon == null)
                {
                    ExtendedEffects.Log($"SLEx_CustomImbueVFX Current Weapon is null");
                    return;
                }

            }
            else
            {
                ExtendedEffects.Log($"SLEx_CustomImbueVFX Target is OffHand");
                if (OffHand == null)
                {
                    ExtendedEffects.Log($"SLEx_CustomImbueVFX OffHand EquipmentSlot cannot be found");
                    return;
                }


                if (!OffHand.HasItemEquipped)
                {
                    ExtendedEffects.Log($"SLEx_CustomImbueVFX OffHand EquipmentSlotitem is null");
                    return;
                }

                CurrentWeapon = OffHand.EquippedItem;
            }


            GameObject Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);

            if (Prefab != null)
            {
                ParticleSystem[] particleSystem = Prefab.GetComponentsInChildren<ParticleSystem>();

                if (particleSystem.Length == 0)
                {
                    ExtendedEffects.Log($"SLEx_CustomImbueVFX prefab {PrefabName} and its children do not contain a particle system.");
                    return;
                }

                MeshFilter meshFilter = OutwardHelpers.TryGetWeaponMesh(CurrentWeapon, false);

                if (meshFilter == null)
                {
                    ExtendedEffects.Log($"SLEx_CustomImbueVFX Mesh Filter For  {CurrentWeapon.Name} cant be found.");
                    return;
                }


                if (Instance == null)
                {
                    Instance = GameObject.Instantiate(Prefab);
                    //ExtendedEffects.Log.LogMessage($"SLEx_CustomImbueVFX spawning instance of {Prefab.name} for {CurrentWeapon.Name}");
                    Instance.transform.parent = CurrentWeapon.LoadedVisual.transform;
        


                    if (RotationOffset == Vector3.zero)
                    {
                        //this is apparently the rotation required with a prefab that is at 0,0,0 with 0,0,0 rotation, why? I dont know, it just is.
                        Instance.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
                    }
                    else
                    {
                        Instance.transform.localEulerAngles = RotationOffset;
                    }

                    Instance.transform.localPosition = PositionOffset;


                    ParticleSystem[] particleSystems = Instance.GetComponentsInChildren<ParticleSystem>();
                    foreach (var ps in particleSystems)
                    {                     
                        if (ps != null)
                        {
                            //ExtendedEffects.Log.LogMessage($"SLEx_CustomImbueVFX Updating ParticleSystem {ps.name} Shape Module Mesh..");
                            ParticleSystem.ShapeModule shapeModule = ps.shape;
                            shapeModule.mesh = meshFilter.mesh;
                            shapeModule.scale = CurrentWeapon.transform.localScale;
                            shapeModule.position = Vector3.zero;
                            shapeModule.rotation = Vector3.zero;
                        }
                    }
                }

            }
            else
            {
                ExtendedEffects.Log($"SLEx_CustomImbueVFX Prefab from AssetBundle {AssetBundleName} was null.");
            }
        }


        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();

            if (Instance)
            {
                ExtendedEffects.Log($"SLEx_CustomImbueVFX Cleaning up Instance");
                GameObject.Destroy(Instance);
            }          
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            if (Instance)
            {
                ExtendedEffects.Log($"SLEx_CustomImbueVFX destroying Instance");
                GameObject.Destroy(Instance);
            }        
        }

    }
}
