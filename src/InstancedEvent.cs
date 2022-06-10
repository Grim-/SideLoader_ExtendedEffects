using System;
using System.Collections.Generic;
using System.Linq;

namespace SideLoader_ExtendedEffects
{
    public class InstancedEvent<TInstance, TEvent> where TEvent : Delegate
    {
        readonly Dictionary<TInstance, TEvent> dictionary = new Dictionary<TInstance, TEvent>();

        public void AddListener(TInstance instance, TEvent listener)
        {
            if (!dictionary.ContainsKey(instance))
                dictionary.Add(instance, listener);
            else
            {
                TEvent _delegate = dictionary[instance];
                _delegate = Delegate.Combine(_delegate, listener) as TEvent;
                dictionary[instance] = _delegate;
            }
        }

        public void RemoveListener(TInstance instance, TEvent listener)
        {
            if (!dictionary.TryGetValue(instance, out TEvent _delegate))
                return;

            _delegate = Delegate.Remove(_delegate, listener) as TEvent;

            if (!_delegate.GetInvocationList().Any())
                dictionary.Remove(instance);
            else
                dictionary[instance] = _delegate;
        }

        internal void InvokeForInstance(TInstance instance, params object[] args)
        {
            if (!dictionary.TryGetValue(instance, out TEvent _delegate))
                return;

            _delegate.DynamicInvoke(args);
        }
    }
}
