using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
    public class AreaTeleporter : BasePortal
    {
        public AreaManager.AreaEnum area;
        public Vector3 Position = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;

        protected override void OnTriggeredTeleporter(Collider other)
        {
            Character character = other.GetComponentInParent<Character>();
            if (character != null && !character.IsAI)
            {
                ExtendedEffects.Instance.PortalManager.StartAreaSwitchAndSetPosition(character, area, Position, Rotation);
            }
        }
    }
}

