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
            comp.ForceSafe = this.ForceSafe;
            comp.NavMeshSampleRadius = this.NavMeshSampleRadius;
        }

        public override void SerializeEffect<T>(T effect)
        {
            base.SerializeEffect(effect);
            var comp = effect as TeleportToPositionEffect;
            this.TargetPosition = comp.TargetPosition;
            this.ForceSafe = comp.ForceSafe;
            this.NavMeshSampleRadius = comp.NavMeshSampleRadius;
        }
    }

    public class TeleportToPositionEffect : TeleportToEffect
    {
        public Vector3 TargetPosition;
        public bool ForceSafe = true;
        public float NavMeshSampleRadius = 5f;

        public override Type SLTemplateModel => typeof(SL_TeleportToPositionEffect);
        public override Type GameModel => typeof(TeleportToPositionEffect);

        private Vector3? GetSafePosition(Vector3 desired)
        {
            if (!ForceSafe) return desired;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(desired, out hit, NavMeshSampleRadius, NavMesh.AllAreas))
            {
                return hit.position;
            }
            return null;
        }

        public override void ActivateLocally(Character affectedCharacter, object[] infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;
            if (character == null || (character != null && character.IsAI))
                return;

            if (character.IsWorldHost && OnlyWorldHost || !OnlyWorldHost)
            {

                if (AreaManager.Instance.CurrentArea.ID != (int)targetArea)
                {
                    NetworkLevelLoader.Instance.RequestSwitchArea(
                        AreaManager.Instance.GetArea(targetArea).SceneName,
                        0, 1.5f, MoveBag);
                    return;
                }


                Vector3? safePos = GetSafePosition(TargetPosition);
                if (safePos.HasValue)
                {
                    affectedCharacter.Teleport(safePos.Value, affectedCharacter.transform.rotation);
                }
                else if (!ForceSafe)
                {
                    affectedCharacter.Teleport(TargetPosition, affectedCharacter.transform.rotation);
                }
            }
        }
    }
}