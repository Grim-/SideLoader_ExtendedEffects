using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_HasPassiveSkill : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_HasPassiveSkill);
        public Type GameModel => typeof(HasPassiveSkillConditionCaster);

        public int PassiveID;

        public override void ApplyToComponent<T>(T component)
        {
            HasPassiveSkillConditionCaster comp = component as HasPassiveSkillConditionCaster;
            comp.PassiveID = PassiveID;
        }

        public override void SerializeEffect<T>(T effect)
        {
            HasPassiveSkillConditionCaster comp = effect as HasPassiveSkillConditionCaster;
            this.PassiveID = comp.PassiveID;
        }
    }

    public class HasPassiveSkillConditionCaster : EffectCondition
    {
        public int PassiveID;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            if (this.m_parentSynchronizer.OwnerCharacter)
            {
                return this.m_parentSynchronizer.OwnerCharacter.Inventory.SkillKnowledge.GetItemFromItemID(PassiveID) != null ? true : false;
            }


            ExtendedEffects.Instance.DebugLogMessage("ParentSynchronizer.OwnerCharacter is null, returning false.");
            return false;
        }
    }
}
