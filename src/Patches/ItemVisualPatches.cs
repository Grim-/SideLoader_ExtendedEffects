using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Patches
{
	public static class ItemVisualPatches
	{
        [HarmonyPatch(typeof(ItemVisual), nameof(ItemVisual.AddImbueFX))]
        public class ItemVisualAddImbueVFXPatch
        {
            static bool Prefix(ItemVisual __instance, ImbueStack newStack, Weapon _linkedWeapon)
            {

                newStack.ImbueFX = ItemManager.Instance.GetImbuedFX(newStack.ImbuedEffectPrefab);

                if (!newStack.ImbueFX.gameObject.activeSelf)
                {
                    newStack.ImbueFX.gameObject.SetActive(true);
                }


                newStack.ParticleSystems = newStack.ImbueFX.GetComponentsInChildren<ParticleSystem>();

                if (OutwardHelpers.DoesWeaponUseSkinnedMesh(_linkedWeapon))
                {

                    SkinnedMeshRenderer skinnedMesh = OutwardHelpers.TryGetFromEquipmentItemVisual<SkinnedMeshRenderer>(_linkedWeapon.EquippedVisuals);

                    //ExtendedEffects._Log.LogMessage("Weapon is a SkinnedMeshRenderer : " + skinnedMesh.transform.name);
                    if (skinnedMesh != default(SkinnedMeshRenderer))
                    {
                        //ExtendedEffects._Log.LogMessage("Found SkinnedMeshRenderer : " + skinnedMesh.transform.name);
                        SetParticleSystemSkinnedMesh(newStack, skinnedMesh);
                    }
                }

                else
                {
                    MeshRenderer meshRenderer = OutwardHelpers.TryGetFromEquipmentItemVisual<MeshRenderer>(_linkedWeapon.EquippedVisuals);
                    if (meshRenderer != null)
                    {
                        SetParticleSystemMesh(newStack, meshRenderer);
                    }

                }

                newStack.ImbueFX.SetParent(__instance.transform);
                newStack.ImbueFX.ResetLocal(true);
                DualMeleeWeapon dualMeleeWeapon;

                if ((dualMeleeWeapon = (__instance.m_item as DualMeleeWeapon)) != null)
                {
                    dualMeleeWeapon.DuplicateImbueFX(newStack);
                }

                __instance.m_linkedImbueFX.Add(newStack);
                return false;
            }

            private static void SetParticleSystemSkinnedMesh(ImbueStack newStack, SkinnedMeshRenderer skinnedMesh)
            {
                for (int j = 0; j < newStack.ParticleSystems.Length; j++)
                {
                    ParticleSystem.MainModule main = newStack.ParticleSystems[j].main;
                    ParticleSystem.ShapeModule Shape = newStack.ParticleSystems[j].shape;
                    Shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
                    Shape.skinnedMeshRenderer = skinnedMesh;
                    newStack.ParticleSystems[j].Play();
                }
            }

            private static void SetParticleSystemMesh(ImbueStack newStack, MeshRenderer meshRenderer)
            {
                for (int j = 0; j < newStack.ParticleSystems.Length; j++)
                {
                    if (newStack.ParticleSystems[j].shape.shapeType == ParticleSystemShapeType.MeshRenderer)
                    {
                        ParticleSystem.MainModule main = newStack.ParticleSystems[j].main;
                        ParticleSystem.ShapeModule Shape = newStack.ParticleSystems[j].shape;
                        Shape.meshRenderer = meshRenderer;
                    }

                    newStack.ParticleSystems[j].Play();
                }
            }
        }
    }
}
