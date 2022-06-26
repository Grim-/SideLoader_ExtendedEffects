using System;
using SideLoader;

namespace SideLoader_ExtendedEffects.Events
{

    public class Publisher<Event> where Event: struct
    {
        public static event EventHandler<Event> Handler;
        public static void RaiseEvent(Event args) {
            SL.Log("Number of handlers: " + Handler.GetInvocationList().Length);
            Handler.Invoke(null, args);
        }
    }

}