using SideLoader.SaveData;
using System.Collections.Generic;
using System.Linq;

namespace SideLoader_ExtendedEffects.Released
{
    public class PortalSaveData : PlayerSaveExtension
    {
        public List<SerializablePortalData> SavedPortals = new List<SerializablePortalData>();

        public override void Save(Character character, bool isWorldHost)
        {
            if (ExtendedEffects.Instance?.PortalManager != null)
            {
                var portalData = ExtendedEffects.Instance.PortalManager.GetSaveData();
                ExtendedEffects._Log.LogMessage($"Saving Portal Data {portalData.Count}");
                SavedPortals = portalData.Select(kvp =>
                    new SerializablePortalData(kvp.Key, kvp.Value)).ToList();
            }
        }

        public override void ApplyLoadedSave(Character character, bool isWorldHost)
        {
            if (ExtendedEffects.Instance?.PortalManager != null)
            {
                var portalDict = SavedPortals.ToDictionary(
                    spd => spd.CharacterUID,
                    spd => spd.PortalData);
                ExtendedEffects._Log.LogMessage($"Loading Portal Data {portalDict.Count}");
                ExtendedEffects.Instance.PortalManager.ApplySaveData(portalDict);
            }
        }
    }

    [System.Serializable]
    public class SerializablePortalData
    {
        public string CharacterUID;
        public ActivePortalData PortalData;

        public SerializablePortalData()
        {
        
        }

        public SerializablePortalData(string uid, ActivePortalData data)
        {
            CharacterUID = uid;
            PortalData = data;
        }
    }
}

