using System;
using UnityEngine;

namespace SideLoader_ExtendedEffects.Released
{
    [Serializable]
    public class PortalData
    {
        public Vector3 Position;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;
        public float RemainingLifetime;
        public float TeleportCooldown;
        public bool CanTeleportPlayers;
        public bool CanTeleportAI;
        public bool CanTeleportProjectiles;
    }

    [Serializable]
    public class ActivePortalData
    {
        public PortalData FirstPortal;
        public PortalData SecondPortal;
        public int AreaID;
        public string SLPackName;
        public string AssetBundleName;
        public string PrefabName;
    }


}

