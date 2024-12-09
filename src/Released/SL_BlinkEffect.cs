using SideLoader;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace SideLoader_ExtendedEffects.Containers
{
    public class SL_BlinkEffect : SL_Effect
    {
        public float BlinkDistance = 5f;
        public float HeightOffset = 1f;
        public LayerMask BlockingLayers;
        public float NavMeshRadius = 0.5f; 

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as BlinkEffect;
            comp.BlinkDistance = this.BlinkDistance;
            comp.HeightOffset = this.HeightOffset;
            comp.BlockingLayers = this.BlockingLayers;
            comp.NavMeshRadius = this.NavMeshRadius;
        }

        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as BlinkEffect;
            this.BlinkDistance = comp.BlinkDistance;
            this.HeightOffset = comp.HeightOffset;
            this.BlockingLayers = comp.BlockingLayers;
            this.NavMeshRadius = comp.NavMeshRadius;
        }
    }


    public class BlinkEffect : Effect, ICustomModel
    {
        public float BlinkDistance = 5f;
        public float HeightOffset = 1f;
        public LayerMask BlockingLayers;
        public float NavMeshRadius = 0.5f;

        public Type SLTemplateModel => typeof(SL_BlinkEffect);
        public Type GameModel => typeof(BlinkEffect);

        private Vector3? GetValidBlinkPosition(Character character)
        {
            Vector3 startPos = character.transform.position + Vector3.up * HeightOffset;
            Vector3 direction = character.transform.forward;

            // First check if there's anything blocking the path
            RaycastHit hit;
            if (Physics.Raycast(startPos, direction, out hit, BlinkDistance, BlockingLayers))
            {
                BlinkDistance = hit.distance - 0.5f;
                if (BlinkDistance <= 0.5f)
                    return null;
            }

            Vector3 targetPos = character.transform.position + (direction * BlinkDistance);


            NavMeshHit navHit;
            if (NavMesh.SamplePosition(targetPos, out navHit, NavMeshRadius, NavMesh.AllAreas))
            {
                if (!Physics.Raycast(navHit.position + Vector3.up * 10f, Vector3.down, out hit, 20f, BlockingLayers))
                {
                    return null;
                }
                return hit.point;
            }

            return null;
        }

        public override void ActivateLocally(Character affectedCharacter, object[] infos)
        {
            if (affectedCharacter == null)
                return;

            Vector3? blinkPos = GetValidBlinkPosition(affectedCharacter);

            if (blinkPos.HasValue)
            {
                affectedCharacter.Teleport(blinkPos.Value, affectedCharacter.transform.rotation);
            }
        }

        public override void CleanUpOnDestroy()
        {
            base.CleanUpOnDestroy();
        }
    }
}