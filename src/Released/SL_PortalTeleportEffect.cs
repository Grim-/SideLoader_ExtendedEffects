using SideLoader;
using System;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
    public class SL_PortalTeleportEffect : SL_Effect
    {
        public float MaxTeleportDistance = 50f;
        public bool TeleportToFarthest = false; 

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as PortalTeleportEffect;
            comp.MaxTeleportDistance = this.MaxTeleportDistance;
            comp.TeleportToFarthest = this.TeleportToFarthest;
        }

        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as PortalTeleportEffect;
            this.MaxTeleportDistance = comp.MaxTeleportDistance;
            this.TeleportToFarthest = comp.TeleportToFarthest;
        }
    }


    public class PortalTeleportEffect : Effect, ICustomModel
    {
        public float MaxTeleportDistance = 50f;
        public bool TeleportToFarthest = false;

        public Type SLTemplateModel => typeof(SL_PortalTeleportEffect);
        public Type GameModel => typeof(PortalTeleportEffect);

        public override void ActivateLocally(Character affectedCharacter, object[] infos)
        {
            Character portalOwner = m_parentSynchronizer.OwnerCharacter;
            if (portalOwner == null || affectedCharacter == null) return;

            var manager = ExtendedEffects.Instance.PortalManager;
            Vector3? targetPosition = manager.GetNearestPortalPosition(
                portalOwner,
                affectedCharacter.transform.position,
                MaxTeleportDistance,
                TeleportToFarthest
            );

            if (targetPosition.HasValue)
            {
                affectedCharacter.Teleport(targetPosition.Value, affectedCharacter.transform.rotation);
            }
        }

        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();
        }
    }
}

