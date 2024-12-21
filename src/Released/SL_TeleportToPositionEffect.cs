using System;
using UnityEngine;
using UnityEngine.AI;

namespace SideLoader_ExtendedEffects.Containers
{
    public class SL_TeleportToPositionEffect : SL_TeleportToEffect
    {
        public Vector3 TargetPosition;
        public bool ForceSafe = true;
        public float NavMeshSampleRadius = 10f;

        public override void ApplyToComponent<T>(T component)
        {
            base.ApplyToComponent(component);
            var comp = component as TeleportToPositionEffect;
            comp.TargetPosition = this.TargetPosition;
        }

        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect(effect);
            var comp = effect as TeleportToPositionEffect;
            this.TargetPosition = comp.TargetPosition;
        }
    }

    public class TeleportToPositionEffect : TeleportToEffect
    {
        public Vector3 TargetPosition;

        public override Type SLTemplateModel => typeof(SL_TeleportToPositionEffect);
        public override Type GameModel => typeof(TeleportToPositionEffect);

        public override void ActivateLocally(Character affectedCharacter, object[] infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;
            if (character == null || (character != null && character.IsAI))
                return;

            if (character.IsWorldHost && OnlyWorldHost || !OnlyWorldHost && !character.IsAI)
            {
                ExtendedEffects.Instance.PortalManager.StartAreaSwitchAndSetPosition(character, targetArea, TargetPosition, Vector3.zero);
            }
        }
    }
}