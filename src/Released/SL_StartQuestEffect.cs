using SideLoader;
using System;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers
{
    public class SL_StartQuestEffect : SL_Effect
    {
        public List<int> questUIDs;

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as StartQuestEffect;
            comp.questUIDs = this.questUIDs;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as StartQuestEffect;
            this.questUIDs = comp.questUIDs;
        }
    }
    public class StartQuestEffect : Effect, ICustomModel
    {
        public List<int> questUIDs;

        public Type SLTemplateModel => typeof(SL_StartQuestEffect);
        public Type GameModel => typeof(StartQuestEffect);

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;

            if (character == null) return;

            foreach (var item in questUIDs)
            {
                GenerateQuestItemForCharacter(character, item);
            }
        }

        public Quest GenerateQuestItemForCharacter(Character character, int QuestItemID)
        {
            Quest quest = ItemManager.Instance.GenerateItemNetwork(QuestItemID) as Quest;
            quest.transform.SetParent(character.Inventory.QuestKnowledge.transform);
            character.Inventory.QuestKnowledge.AddItem(quest);
            return quest;
        }
    }
}