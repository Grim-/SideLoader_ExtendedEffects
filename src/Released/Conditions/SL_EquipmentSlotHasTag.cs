using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    /// <summary>
    /// An EffectCondition that checks if the specified EquipmentSlot has anything equipped and if so does that Equipment have the specified tags, returns false unless all conditions are met.
    /// </summary>
    public class SL_EquipmentSlotHasTag : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_EquipmentSlotHasTag);
        public Type GameModel => typeof(EquipmentSlotHasTag);

        public string EquipmentSlotID;
        public string TagName;

        public override void ApplyToComponent<T>(T component)
        {
            EquipmentSlotHasTag comp = component as EquipmentSlotHasTag;
            comp.EquipmentSlot = (EquipmentSlot.EquipmentSlotIDs) Enum.Parse(typeof(EquipmentSlot.EquipmentSlotIDs), EquipmentSlotID);
            comp.TagName = TagName;
        }

        public override void SerializeEffect<T>(T component)
        {
            EquipmentSlotHasTag comp = component as EquipmentSlotHasTag;
            this.EquipmentSlotID = comp.EquipmentSlot.ToString();
            this.TagName = comp.TagName;
        }
    }

    public class EquipmentSlotHasTag : EffectCondition
    {

        public EquipmentSlot.EquipmentSlotIDs EquipmentSlot;
        public string TagName;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            //ExtendedEffects.Instance.DebugLogMessage($"Checking Slot : {EquipmentSlot} for Tag {TagName}");

            if (_affectedCharacter.Inventory.Equipment.HasItemEquipped(EquipmentSlot))
            {
                Tag FoundTag = OutwardHelpers.GetTagFromName(TagName);
                //ExtendedEffects.Instance.DebugLogMessage($"Found Tag : {FoundTag} uid : {FoundTag.UID}");
                if (FoundTag != default(Tag))
                {
                    Equipment EquippedItem = _affectedCharacter.Inventory.Equipment.GetMatchingSlot(EquipmentSlot).EquippedItem;
                    return EquippedItem != null ? EquippedItem.HasTag(FoundTag) : false;
                }
                else
                {
                    ExtendedEffects.Instance.DebugLogMessage($"Couldn't find tag with name {TagName}");
                }
            }

            ExtendedEffects.Instance.DebugLogMessage($"Has no equipped Item in the specified slot {EquipmentSlot}");
            return false;
        }
    }
}
