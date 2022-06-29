using SideLoader_ExtendedEffects.Events;
using SideLoader;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace SideLoader_ExtendedEffects.Containers.Triggers {

    public abstract class TriggeredEffect<Event>: ParentEffect where Event: struct
    {
        private bool registered = false;
        private float lastTriggerTime = 0;
        private List<Character> affectedThisInterval;
        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (!registered) {
                SL.Log("Registering Event");
                SL.Log(((EventHandler<Event>)OnEvent).Method.DeclaringType);
                Publisher<Event>.Handler += OnEvent;
                registered = true;
            } else {
                SL.Log("Event already registered");
            }
        }

        public override void StopAffectLocally(Character _affectedCharacter)
        { 
            if (registered) {
                SL.Log("Deregistering Event");
                Publisher<Event>.Handler -= OnEvent;
                registered = false;
            } else {
                SL.Log("Event not registered");
            }
            base.StopAffectLocally(_affectedCharacter);
        }
        
        public virtual void StartApply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter, Vector3 pos, Vector3 dir) {
            if (Time.time - this.lastTriggerTime > 0)
            {
                this.lastTriggerTime = Time.time;
                if (this.affectedThisInterval == null)
                {
                    this.affectedThisInterval = new List<Character>();
                }
                else
                {
                    this.affectedThisInterval.Clear();
                }
            }
            if (!this.affectedThisInterval.Contains(affectedCharacter))
            {
                this.affectedThisInterval.Add(affectedCharacter);
                foreach (var category in categories)
                {
                    this.m_subEffects[0].SynchronizeEffects(category, affectedCharacter, pos, dir);
                }
            }
        }
        public virtual void StopApply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter)
        {
            foreach (var category in categories)
            {
                this.m_subEffects[0].StopAllEffects(category, affectedCharacter);
            }
        }

        public virtual void Apply(EffectSynchronizer.EffectCategories[] categories, Character affectedCharacter, Vector3 pos, Vector3 dir) {
            StartApply(categories, affectedCharacter, pos, dir);
            StopApply(categories, affectedCharacter);
        }

        public abstract void OnEvent(object sender, Event args);
    }

}