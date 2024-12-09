using SideLoader;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace SideLoader_ExtendedEffects.Containers
{
    public class SL_TeleportToEffect : SL_Effect
    {
        public AreaManager.AreaEnum targetArea;
        public bool OnlyWorldHost = true;
        public bool MoveBag = true;

        public override void ApplyToComponent<T>(T component)
        {
            var comp = component as TeleportToEffect;
            comp.targetArea = this.targetArea;
            comp.OnlyWorldHost = this.OnlyWorldHost;
            comp.MoveBag = this.MoveBag;
        }
        public override void SerializeEffect<T>(T effect)
        {
            var comp = effect as TeleportToEffect;
            this.targetArea = comp.targetArea;
            this.targetArea = comp.targetArea;
            this.OnlyWorldHost = comp.OnlyWorldHost;
            this.MoveBag = comp.MoveBag;
        }
    }

    public class TeleportToEffect : Effect, ICustomModel
    {
        public AreaManager.AreaEnum targetArea;
        public bool OnlyWorldHost = true;
        public bool MoveBag = true;

        public virtual Type SLTemplateModel => typeof(SL_TeleportToEffect);
        public virtual Type GameModel => typeof(TeleportToEffect);

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            Character character = this.m_parentSynchronizer.OwnerCharacter;

            if (character == null || character != null && character.IsAI)
                return;

            if (character.IsWorldHost && OnlyWorldHost)
            {
                NetworkLevelLoader.Instance.RequestSwitchArea(AreaManager.Instance.GetArea(targetArea).SceneName, 0, 1.5f, MoveBag);
            }
            else
            {
                NetworkLevelLoader.Instance.RequestSwitchArea(AreaManager.Instance.GetArea(targetArea).SceneName, 0, 1.5f, MoveBag);
            }

        }
    }

}