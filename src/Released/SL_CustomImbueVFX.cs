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
        private Equipment LastEquipmentInstance = null;
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (_affectedCharacter == null)
            {
                return;
            }

            //This has been called already if this isnt null
            if (LastEquipmentInstance != null)
            {
                if (_affectedCharacter.CurrentWeapon != LastEquipmentInstance)
                {
                    //ExtendedEffects.Instance?.Log($"CustomImbueVFX Affected Character CurrentWeapon is not equal to the LastEquipmentInstance, destroying VFX Instance if it exists.");

                    if (Instance != null)
                    {
                        GameObject.Destroy(Instance);
                    }

                    LastEquipmentInstance = null;
                }           
            }


            Equipment CurrentWeapon = FindCurrentWeapon(_affectedCharacter);

            if (CurrentWeapon == null)
            {
                return;
            }

            GameObject Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(SLPackName, AssetBundleName, PrefabName);

            if (Prefab != null)
            {
                if (Instance == null)
                {
                    Instance = GameObject.Instantiate(Prefab);
                }


                ParticleSystem[] particleSystem = Prefab.GetComponentsInChildren<ParticleSystem>();

                if (particleSystem.Length == 0)
                {
                    ExtendedEffects.Instance?.DebugLogMessage($"SLEx_CustomImbueVFX prefab {PrefabName} and its children do not contain a particle system.");
                    return;
                }


                if (OutwardHelpers.DoesWeaponUseSkinnedMesh(CurrentWeapon))
                {
                    ExtendedEffects.Instance?.DebugLogMessage($"SLEx_CustomImbueVFX Weapon is SkinnedMeshRenderer");

                    SkinnedMeshRenderer skinnedMeshRenderer = OutwardHelpers.TryGetFromEquipmentItemVisual<SkinnedMeshRenderer>(CurrentWeapon.EquippedVisuals);

                    if (skinnedMeshRenderer == null)
                    {
                        ExtendedEffects.Instance?.DebugLogMessage($"SLEx_CustomImbueVFX skinnedMeshRenderer For {CurrentWeapon.Name} cant be found.");
                        return;
                    }

                }
                else
                {
                    ExtendedEffects.Instance?.DebugLogMessage($"SLEx_CustomImbueVFX Weapon is Static Mesh");
                    MeshFilter meshFilter = OutwardHelpers.TryGetFromEquipmentItemVisual<MeshFilter>(CurrentWeapon.EquippedVisuals);

                    if (meshFilter == null)
                    {
                        ExtendedEffects.Instance?.DebugLogMessage($"SLEx_CustomImbueVFX Mesh Filter For  {CurrentWeapon.Name} cant be found.");
                        return;
                    }

                    UpdateParticleSystemMesh(Instance, CurrentWeapon, meshFilter);
                }

                LastEquipmentInstance = CurrentWeapon;
            }
            else
            {
                ExtendedEffects.Instance?.DebugLogMessage($"SLEx_CustomImbueVFX Prefab from AssetBundle {AssetBundleName} was null.");
            }
        }

        private void UpdateParticleSystemMesh(GameObject Instance, Equipment CurrentWeapon, MeshFilter meshFilter)
        {
            ParticleSystem[] particleSystems = Instance.GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in particleSystems)
            {
                if (ps != null)
                {
                    ParticleSystem.ShapeModule shapeModule = ps.shape;

                    if (ps.shape.shapeType == ParticleSystemShapeType.MeshRenderer || ps.shape.shapeType == ParticleSystemShapeType.Mesh)
                    {
                        shapeModule.mesh = meshFilter.sharedMesh;
                        shapeModule.meshRenderer = meshFilter.GetComponent<MeshRenderer>();
                        shapeModule.scale = CurrentWeapon.transform.localScale;
                        shapeModule.position = Vector3.zero;
                        shapeModule.rotation = Vector3.zero;
                    }
                }
            }

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
        }

        private void UpdateParticleSystemMesh(GameObject Instance, Equipment CurrentWeapon, SkinnedMeshRenderer skinnedMeshRenderer)
        {
            ParticleSystem[] particleSystems = Instance.GetComponentsInChildren<ParticleSystem>();

            foreach (var ps in particleSystems)
            {
                if (ps != null)
                {
                    ParticleSystem.ShapeModule shapeModule = ps.shape;

                    if (ps.shape.shapeType == ParticleSystemShapeType.SkinnedMeshRenderer)
                    {
                        shapeModule.skinnedMeshRenderer = skinnedMeshRenderer;
                        shapeModule.scale = CurrentWeapon.transform.localScale;
                        shapeModule.position = Vector3.zero;
                        shapeModule.rotation = Vector3.zero;
                    }
                }
            }

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
        }

        private Equipment FindCurrentWeapon(Character _affectedCharacter)
        {
            Equipment CurrentWeapon = null;
            EquipmentSlot OffHand = _affectedCharacter.Inventory.Equipment.GetMatchingSlot(EquipmentSlot.EquipmentSlotIDs.LeftHand);

            if (IsMainHand)
            {
                //ExtendedEffects.Instance?.Log($"SLEx_CustomImbueVFX Target is MainHand");

                if (_affectedCharacter.CurrentWeapon == null)
                {
                    ExtendedEffects.Instance.DebugLogMessage($"SLEx_CustomImbueVFX Current Weapon is null");
                    return null;
                }

                CurrentWeapon = _affectedCharacter.CurrentWeapon;
            }
            else
            {
                //ExtendedEffects.Instance?.Log($"SLEx_CustomImbueVFX Target is OffHand");
                if (OffHand == null)
                {
                    ExtendedEffects.Instance.DebugLogMessage($"SLEx_CustomImbueVFX OffHand EquipmentSlot cannot be found");
                    return null;
                }


                if (!OffHand.HasItemEquipped)
                {
                    ExtendedEffects.Instance?.DebugLogMessage($"SLEx_CustomImbueVFX OffHand EquipmentSlotitem is null");
                    return null;
                }

                CurrentWeapon = OffHand.EquippedItem;
            }

            return CurrentWeapon;
        }


        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();

            if (Instance)
            {
                ExtendedEffects.Instance.DebugLogMessage($"SLEx_CustomImbueVFX Cleaning up Instance");
                GameObject.Destroy(Instance);
            }          
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        {
            if (Instance)
            {
                ExtendedEffects.Instance.DebugLogMessage($"SLEx_CustomImbueVFX destroying Instance");
                GameObject.Destroy(Instance);
            }        
        }

    }
}
