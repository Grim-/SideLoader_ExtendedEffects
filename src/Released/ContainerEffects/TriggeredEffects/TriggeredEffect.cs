using SideLoader_ExtendedEffects.Events;
using SideLoader;
using System;

namespace SideLoader_ExtendedEffects.Containers.Triggers {

    public abstract class TriggeredEffect<Event>: ParentEffect where Event: struct
    {
        private bool registered = false;
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

        public abstract void OnEvent(object sender, Event args);
    }

}