using SideLoader;
using SideLoader_ExtendedEffects.Released.Conditions;
using System;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
    public class SL_PortalEffect : SL_Effect
    {
        public string SLPackName = "sideloaderextended-magiccircles";
        public string AssetBundleName = "magiccircles";
        public string PrefabName = "MagicCircle";
        public Vector3 PositionOffset;
        public Vector3 RotationOffset = new Vector3(0, -90, 0);
        public float PortalLifeTime = -1;
        public float PortalTeleportCooldown = 5f;
        public bool canTeleportAI = true;
        public bool canTeleportPlayers = true;
        public bool canTeleportProjectiles = true;

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as PortalEffect;
            comp.SLPackName = this.SLPackName;
            comp.AssetBundleName = this.AssetBundleName;
            comp.PrefabName = this.PrefabName;
            comp.PositionOffset = this.PositionOffset;
            comp.RotationOffset = this.RotationOffset;
        }

        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as PortalEffect;
            this.SLPackName = comp.SLPackName;
            this.AssetBundleName = comp.AssetBundleName;
            this.PrefabName = comp.PrefabName;
            this.PositionOffset = comp.PositionOffset;
            this.RotationOffset = comp.RotationOffset;
        }
    }
    public class PortalEffect : Effect, ICustomModel
    {
        public string SLPackName = "sideloaderextended-magiccircles";
        public string AssetBundleName = "magiccircles";
        public string PrefabName = "MagicCircle";
        public Vector3 PositionOffset = Vector3.zero;
        public Vector3 RotationOffset = new Vector3(0, -90, 0);
        public float PortalLifeTime = -1;
        public float PortalTeleportCooldown = 5f;
        public bool canTeleportAI = true;
        public bool canTeleportPlayers = true;
        public bool canTeleportProjectiles = true;

        public Type SLTemplateModel => typeof(SL_PortalEffect);
        public Type GameModel => typeof(PortalEffect);

        public override void ActivateLocally(Character affectedCharacter, object[] infos)
        {
            Character character = m_parentSynchronizer.OwnerCharacter;
            if (character == null) return;
            var manager = ExtendedEffects.Instance.PortalManager;

            if (manager.HasSecondPortal(character))
            {
                manager.ClearPortals(character);
                manager.PlaceFirstPortal(character, character.transform.position, SLPackName, AssetBundleName, PrefabName, PositionOffset, RotationOffset, PortalLifeTime, PortalTeleportCooldown, canTeleportAI, canTeleportPlayers, canTeleportProjectiles);
            }
            else if (manager.HasFirstPortal(character))
            {
                manager.PlaceSecondPortal(character, character.transform.position, SLPackName, AssetBundleName, PrefabName, PositionOffset, RotationOffset, PortalLifeTime, PortalTeleportCooldown, canTeleportAI, canTeleportPlayers, canTeleportProjectiles);
            }
            else
            {
                manager.PlaceFirstPortal(character, character.transform.position, SLPackName, AssetBundleName, PrefabName, PositionOffset, RotationOffset, PortalLifeTime, PortalTeleportCooldown, canTeleportAI, canTeleportPlayers, canTeleportProjectiles);
            }
        }

        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();
        }
    }

}

